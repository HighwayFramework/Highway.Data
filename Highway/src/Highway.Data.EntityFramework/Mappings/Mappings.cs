using System.Data.Entity.Core.Metadata.Edm;

namespace Highway.Data
{
    public static class Mappings
    {
        /// <summary>
        /// Creates a mapping for the type provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TypeMappings FromAssemblyContaing<T>()
        {
            return new TypeMappings().FromAssemblyContaing<T>();
        }

        public static TypeMappings FromAssemblyContaing<T>(this TypeMappings mappings)
        {
            mappings.AddType<T>();
            return mappings;
        }

    }
}