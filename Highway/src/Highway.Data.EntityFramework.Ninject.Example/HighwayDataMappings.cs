using System.Data.Entity;
using Highway.Data.EntityFramework.Ninject.Example.Domain;

namespace Highway.Data.EntityFramework.Ninject.Example
{
    public class HighwayDataMappings : IMappingConfiguration
    {
        #region IMappingConfiguration Members

        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
        }

        #endregion
    }
}