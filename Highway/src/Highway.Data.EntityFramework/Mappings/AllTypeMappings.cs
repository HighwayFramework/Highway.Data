using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;

namespace Highway.Data
{
    /// <summary>
    /// Mappings for the assembly containing the type provided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AllTypeMappings : IMappingConfiguration
    {
        /// <summary>
        /// Loads all assemblies to discover mappings
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            List<Assembly> added = new List<Assembly>();
            foreach (var assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Assembly assembly = Assembly.Load(assemblyName);
                if (added.Contains(assembly)) continue;
                modelBuilder.Configurations.AddFromAssembly(assembly);
                added.Add(assembly);
            }
            added = null;
        }
    }
}