using System.Data.Entity;

namespace Highway.Data.EntityFramework.Test.TestDomain
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
