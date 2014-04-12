using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.Utilities
{
    public static class CloneExtension
    {
        private static Dictionary<object, object> originalToCloneMap;

        public static Dictionary<object, object> CloneMap<T>(this T originalObject) where T : class
        {
            return CloneMap<T>(originalObject, new Dictionary<object, object>());
        }

        public static Dictionary<object, object> CloneMap<T>(this T originalObject, Dictionary<object, object> existingOriginalToCloneMap) where T : class
        {
            originalToCloneMap = existingOriginalToCloneMap ?? new Dictionary<object, object>();

            ExecuteClone(originalObject);

            return originalToCloneMap;
        }

        public static T Clone<T>(this T originalObject) where T : class
        {
            originalToCloneMap = new Dictionary<object, object>();

            var cloneObject = ExecuteClone(originalObject);

            return cloneObject;
        }

        public static T ExecuteClone<T>(this T originalObject) where T : class
        {
            if(originalToCloneMap.ContainsKey(originalObject))
                return (T)originalToCloneMap[originalObject];

            var cloneObject = (T)InstantiateClone(originalObject);
            
            if(!typeof(IEnumerable).IsAssignableFrom(originalObject.GetType()))
                CloneFields(originalObject, cloneObject);

            return cloneObject;
        }

        private static void CloneFields<T>(T originalObject, T cloneObject)
        {
            Type type = originalObject.GetType();

            do
            {
                CloneFieldsForType<T>(originalObject, cloneObject, type);

                type = type.BaseType;
            } while (type != null);
        }

        private static void CloneFieldsForType<T>(T originalObject, T cloneObject, Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var fieldInfo = type.GetField(field.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                var value = fieldInfo.GetValue(originalObject);

                if (value == null) continue;

                if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(Guid)) fieldInfo.SetValue(cloneObject, value);
                else
                {
                    var methodInfo = typeof(CloneExtension).GetMethod("ExecuteClone");
                    var genericMethod = methodInfo.MakeGenericMethod(value.GetType());

                    fieldInfo.SetValue(cloneObject, genericMethod.Invoke(value, new object[] { value }));
                }
            }
        }

        private static T InstantiateClone<T>(T originalObject)
        {
            if (typeof(IEnumerable).IsAssignableFrom(originalObject.GetType()))
                return InstantiateCollectionClone(originalObject);
            else
                return InstantiateClassClone(originalObject);
        }

        private static T InstantiateClassClone<T>(T classObject)
        {
            Type t = classObject.GetType();

            ConstructorInfo ci = t.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                System.Type.DefaultBinder, System.Type.EmptyTypes, null);

            T cloneObject;

            try
            {
                cloneObject = (T)ci.Invoke(null);
            }
            catch (NullReferenceException e)
            {
                throw new MissingMethodException(string.Format("Possible missing default constructor for {0}. Can be private. Required for EF as well.", t), e);
            }
            
            originalToCloneMap.Add(classObject, cloneObject);
            return cloneObject;
        }

        private static T InstantiateCollectionClone<T>(T originalCollection)
        {
            var collectionType = originalCollection.GetType().GetGenericTypeDefinition();
            Type genericType = collectionType.MakeGenericType(
                originalCollection.GetType().GetGenericArguments());
            var cloneCollection = (T)Activator.CreateInstance(genericType);

            var isIListType = typeof(IList).IsAssignableFrom(collectionType);

            originalToCloneMap.Add(originalCollection, cloneCollection);

            foreach (var item in (IEnumerable)originalCollection)
            {
                if(isIListType)
                    ((IList)cloneCollection).Add(ExecuteClone(item));
                else
                    throw new NotSupportedException("Uncertain of what other collection types to handle.");
            }
            
            return cloneCollection;
        }
    }
}
