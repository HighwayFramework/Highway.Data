using Highway.Data.EntityFramework.Ninject.Example.Domain;

namespace Highway.Data.EntityFramework.Ninject.Example
{
    public class HighwayDataMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
        }
    }
}
