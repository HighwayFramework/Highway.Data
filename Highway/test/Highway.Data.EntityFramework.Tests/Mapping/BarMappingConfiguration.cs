#region

using System.Data.Entity;

#endregion

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