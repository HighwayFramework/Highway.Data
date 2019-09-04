using System;
using System.Data.Entity;

namespace Highway.Data.EntityFramework.Test.TestDomain
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
