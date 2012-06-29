using System.Data.Entity;
using Highway.Data.EntityFramework.Mappings;

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class TestMappingConfiguration : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FooMap());
        }
    }
}