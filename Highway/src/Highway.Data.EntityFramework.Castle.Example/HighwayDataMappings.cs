using Highway.Data.EntityFramework.Castle.Example.Domain;

namespace Highway.Data.EntityFramework.Castle.Example
{
    public class HighwayDataMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
        }
    }
}
