using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Highway.Data.Collections
{
    [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
    public static class CloneExtension
    {
        private static IDictionary<object, object> OriginalToCloneMap;

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        public static T Clone<T>(this T originalObject, IDictionary<object, object> existingOriginalToCloneMap)
            where T : class
        {
            OriginalToCloneMap = existingOriginalToCloneMap ?? new Dictionary<object, object>();

            return ExecuteClone(originalObject);
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        public static T Clone<T>(this T originalObject)
            where T : class
        {
            return Clone(originalObject, null);
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        public static T ExecuteClone<T>(this T originalObject)
            where T : class
        {
            if (OriginalToCloneMap.ContainsKey(originalObject))
            {
                return (T)OriginalToCloneMap[originalObject];
            }

            var cloneObject = InstantiateClone(originalObject);

            if (originalObject is IEnumerable)
            {
                return cloneObject;
            }

            CloneFields(originalObject, cloneObject);

            return cloneObject;
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        private static void CloneFields<T>(T originalObject, T cloneObject)
        {
            var type = originalObject.GetType();

            do
            {
                CloneFieldsForType(originalObject, cloneObject, type);

                type = type.BaseType;
            }
            while (type != null);
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        private static void CloneFieldsForType<T>(T originalObject, T cloneObject, Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var fieldInfo = type.GetField(field.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                var value = fieldInfo.GetValue(originalObject);

                if (value == null)
                {
                    continue;
                }

                if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(Guid))
                {
                    fieldInfo.SetValue(cloneObject, value);
                }
                else
                {
                    var methodInfo = typeof(CloneExtension).GetMethod("ExecuteClone");
                    var genericMethod = methodInfo.MakeGenericMethod(value.GetType());

                    fieldInfo.SetValue(cloneObject, genericMethod.Invoke(value, new[] { value }));
                }
            }
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        private static T InstantiateClassClone<T>(T classObject)
        {
            var t = classObject.GetType();

            var ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                Type.DefaultBinder,
                Type.EmptyTypes,
                null);

            T cloneObject;

            try
            {
                cloneObject = (T)ci.Invoke(null);
            }
            catch (NullReferenceException e)
            {
                throw new MissingMethodException($"Possible missing default constructor for {t}. Can be private. Required for EF as well.", e);
            }

            OriginalToCloneMap.Add(classObject, cloneObject);

            return cloneObject;
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        private static T InstantiateClone<T>(T originalObject)
        {
            return originalObject is IEnumerable
                ? InstantiateCollectionClone(originalObject)
                : InstantiateClassClone(originalObject);
        }

        [Obsolete("This will be used in a future version.  Use Highway.Data.Utilities.CloneExtension instead.")]
        private static T InstantiateCollectionClone<T>(T originalCollection)
        {
            var collectionType = originalCollection.GetType().GetGenericTypeDefinition();
            var genericType = collectionType.MakeGenericType(originalCollection.GetType().GetGenericArguments());
            var cloneCollection = (T)Activator.CreateInstance(genericType);

            if (!typeof(IList).IsAssignableFrom(collectionType))
            {
                throw new NotSupportedException("Uncertain of what other collection types to handle.");
            }

            OriginalToCloneMap.Add(originalCollection, cloneCollection);

            foreach (var item in (IEnumerable)originalCollection)
            {
                ((IList)cloneCollection).Add(ExecuteClone(item));
            }

            return cloneCollection;
        }
    }
}
