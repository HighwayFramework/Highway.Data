using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Castle.Core.Logging;
using Highway.Data;

namespace Templates.Models
{
    // Remove the obsolete attribute once you've addressed this change.
    // TODO Change the base class for this to an Initializer that matches your strategy.
    public class DatabaseInitializer : DropCreateDatabaseAlways<DataContext>
    {
        public ILogger Logger { get; set; }

        public DatabaseInitializer() 
        {
            Logger = NullLogger.Instance;
        }

        protected override void Seed(DataContext context)
        {
            Logger.Info("Seeding Database");
            base.Seed(context);
        }
    }
}
