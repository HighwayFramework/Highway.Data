using System.Data.Entity;
using Highway.Data.EntityFramework.Mappings;

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class QuxMappingConfiguration : BaseMappingConfiguration
    {
        public override void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new QuxMap());
            base.ConfigureModelBuilder(modelBuilder);

        }
    }
}
