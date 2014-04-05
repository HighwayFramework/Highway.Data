using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.Utilities
{
    public static class CloneExtension
    {
        private static Dictionary<object, object> originalToCloneMapping = new Dictionary<object, object>();
        public static T Clone<T>(this T originalObject) where T : class
        {
            var cloneObject = ExecuteClone(originalObject);
            originalToCloneMapping.Clear();
            return cloneObject;
        }

        public static T ExecuteClone<T>(this T originalObject) where T : class
        {
            if(originalToCloneMapping.ContainsKey(originalObject))
                return (T)originalToCloneMapping[originalObject];

            var cloneObject = InstantiateClone(originalObject);
            originalToCloneMapping.Add(originalObject, cloneObject);
            CloneFields(originalObject, cloneObject);

            return cloneObject;
        }

        private static void CloneFields<T>(T originalObject, T cloneObject)
        {
            Type type = typeof(T);

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

                if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string)) fieldInfo.SetValue(cloneObject, value);
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
            Type t = typeof(T);

            ConstructorInfo ci = t.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                System.Type.DefaultBinder, System.Type.EmptyTypes, null);

            return (T)ci.Invoke(null);
        }
    }
}
