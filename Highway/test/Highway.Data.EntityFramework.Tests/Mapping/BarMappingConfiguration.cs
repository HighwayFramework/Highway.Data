using System.Data.Entity;
using Highway.Data.EntityFramework.Mappings;

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class BarMappingConfiguration : BaseMappingConfiguration
    {
        public override void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BarMap());
            base.ConfigureModelBuilder(modelBuilder);
        }
    }
}
