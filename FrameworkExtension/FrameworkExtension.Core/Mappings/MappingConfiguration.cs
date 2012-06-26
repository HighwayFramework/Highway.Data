using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace FrameworkExtension.Core.Mappings
{
    public abstract class MappingConfiguration
    {
        public abstract void ConfigureModelBuilder(DbModelBuilder modelBuilder);
    }
}