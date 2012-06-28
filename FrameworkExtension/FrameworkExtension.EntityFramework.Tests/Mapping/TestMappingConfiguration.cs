using System.Data.Entity;
using FrameworkExtension.EntityFramework.Mappings;

namespace FrameworkExtension.EntityFramework.Tests.Mapping
{
    public class TestMappingConfiguration : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FooMap());
        }
    }
}