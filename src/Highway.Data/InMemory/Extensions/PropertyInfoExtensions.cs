using System.Reflection;

namespace Highway.Data.InMemory.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool TrySetValue(this PropertyInfo propertyInfo, object obj, object value, object[] index)
        {
            if (!propertyInfo.CanWrite)
            {
                return false;
            }

            propertyInfo.SetValue(obj, value, index);

            return true;
        }
    }
}
