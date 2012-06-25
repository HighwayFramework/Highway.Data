using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Test.EntityFramework.Mapping;
using FrameworkExtension.Core.Test.Properties;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    public class EntityFrameworkTestContext : EntityFrameworkContext
    {
        public EntityFrameworkTestContext() : base(Settings.Default.Connection)
        {
            
        }
        
        public EntityFrameworkTestContext(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FooMap());
            base.OnModelCreating(modelBuilder);
        }

        public string ConnectionString { get; set; }
    }
}