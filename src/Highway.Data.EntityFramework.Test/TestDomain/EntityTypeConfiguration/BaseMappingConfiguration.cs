﻿using System.Data.Entity;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class BaseMappingConfiguration : IMappingConfiguration
    {
        public BaseMappingConfiguration()
        {
            Configured = false;
        }

        public bool Configured { get; set; }

        public virtual void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            Configured = true;
        }
    }
}
