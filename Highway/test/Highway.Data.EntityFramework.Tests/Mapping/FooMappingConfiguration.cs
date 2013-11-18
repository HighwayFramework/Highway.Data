#region

using System.Data.Entity;

#endregion

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class FooMappingConfiguration : BaseMappingConfiguration
    {
        public override void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FooMap());
            base.ConfigureModelBuilder(modelBuilder);
        }
    }
}