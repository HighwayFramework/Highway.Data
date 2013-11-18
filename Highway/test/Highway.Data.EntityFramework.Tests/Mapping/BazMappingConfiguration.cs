#region

using System.Data.Entity;

#endregion

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