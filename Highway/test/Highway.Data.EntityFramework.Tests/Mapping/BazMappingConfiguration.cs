using System.Data.Entity;
using Highway.Data.EntityFramework.Mappings;

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class BazMappingConfiguration : BaseMappingConfiguration
    {
        public override void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BazMap());
            base.ConfigureModelBuilder(modelBuilder);
        }
    }
}
