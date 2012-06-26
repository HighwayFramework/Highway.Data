using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Mappings;
using FrameworkExtension.Core.Test.EntityFramework.Mapping;
using FrameworkExtension.Core.Test.Properties;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    public class EntityFrameworkTestContext : EntityFrameworkContext
    {
        public EntityFrameworkTestContext(string connectionString, MappingConfiguration configuration) : base(connectionString, configuration)
        {
        }

        public string ConnectionString { get; set; }
    }
}