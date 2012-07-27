using Highway.Data.EntityFramework.StructureMap.Example.Domain;

namespace Highway.Data.EntityFramework.StructureMap.Example
{
    public class HighwayDataMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
        }
    }
}
