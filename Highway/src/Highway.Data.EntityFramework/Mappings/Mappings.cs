using System.Data.Entity;
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
        public static IMappingConfiguration CreateFromAssembly<T>()
        {
            return new AssemblyMappings<T>();
        }
    }

    /// <summary>
    /// Mappings for the assembly containing the type provided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssemblyMappings<T> : IMappingConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(T).Assembly);
        }
    }
}