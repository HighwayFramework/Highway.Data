using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Highway.Data
{
    public class DbModelBuilderAdapter : DbModelBuilder, IDbModelBuilderAdapter
    {
        public new IConfigurationRegistrarAdapater Configurations
        {
            get { return new ConfigurationRegistrarAdapater(base.Configurations); }
        }
    }
}