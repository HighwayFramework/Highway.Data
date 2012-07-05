using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EntityFramework.Contexts;
using System.Data.Entity;
using Castle.Core.Logging;

namespace Templates.Models
{
    // Remove the obsolete attribute once you've addressed this change.
    // TODO Change the base class for this to an Initializer that matches your strategy.
    public class DatabaseInitializer : DropCreateDatabaseAlways<EntityFrameworkContext>
    {
        public ILogger Logger { get; set; }

        public DatabaseInitializer() 
        {
            Logger = NullLogger.Instance;
        }

        protected override void Seed(EntityFrameworkContext context)
        {
            Logger.Info("Seeding Database");
            base.Seed(context);
        }
    }
}
