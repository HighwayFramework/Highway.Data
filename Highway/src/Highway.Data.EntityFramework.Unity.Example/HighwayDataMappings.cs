using Highway.Data.EntityFramework.Unity.Example.Domain;

namespace Highway.Data.EntityFramework.Unity.Example
{
    public class HighwayDataMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
        }
    }
}
