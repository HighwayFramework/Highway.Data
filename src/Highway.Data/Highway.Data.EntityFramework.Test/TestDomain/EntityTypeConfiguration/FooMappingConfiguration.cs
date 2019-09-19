using System;
using System.Data.Entity;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class FooMappingConfiguration : BaseMappingConfiguration
    {
        public override void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            Called = true;
            modelBuilder.Configurations.Add(new FooMap());
            base.ConfigureModelBuilder(modelBuilder);
        }

        public bool Called { get; set; } = false;
    }
}
