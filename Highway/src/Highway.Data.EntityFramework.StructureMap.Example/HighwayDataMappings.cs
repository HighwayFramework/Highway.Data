using System.Data.Entity;
using Highway.Data.EntityFramework.StructureMap.Example.Domain;

namespace Highway.Data.EntityFramework.StructureMap.Example
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