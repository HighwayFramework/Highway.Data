using System.Data.Entity;

namespace Highway.Data.EntityFramework.Mappings
{
    /// <summary>
    /// Implement this interface to pass the mappings in via constructor injection on the context.
    /// </summary>
    public interface IMappingConfiguration
    {
        /// <summary>
        /// This method takes the modelBuilder from Entity Framework and wires in the mappings provided
        /// </summary>
        /// <param name="modelBuilder">The Database model builder used by Entity Framework to generate the model.</param>
        void ConfigureModelBuilder(DbModelBuilder modelBuilder);
    }
}