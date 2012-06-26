using System.Data.Entity;
using FrameworkExtension.Core.Mappings;

namespace FrameworkExtension.Core.Test.EntityFramework.Mapping
{
    public class TestMappingConfiguration : MappingConfiguration
    {
        public override void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FooMap());
        }
    }
}