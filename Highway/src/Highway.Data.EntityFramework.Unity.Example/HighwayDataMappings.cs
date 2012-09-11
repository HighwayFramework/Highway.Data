using System.Data.Entity;
using Highway.Data.EntityFramework.Unity.Example.Domain;

namespace Highway.Data.EntityFramework.Unity.Example
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