using System.Data.Entity;
using FrameworkExtension.Core.Test.EntityFramework.UnitTests;
using FrameworkExtension.Core.Test.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrameworkExtension.Core.Test.EntityFramework.Initializer
{
    public class EntityFrameworkIntializer : DropCreateDatabaseAlways<EFTestContext>
    {
        protected override void Seed(EFTestContext context)
        {
            for (int i = 0; i < 5;i++ )
            {
                context.Add(new Foo());
            }
            context.SaveChanges();
        }
    }

    public class ForceDeleteInitializer : IDatabaseInitializer<EFTestContext>
    {
        private readonly IDatabaseInitializer<EFTestContext> _initializer;

        public ForceDeleteInitializer(IDatabaseInitializer<EFTestContext> innerInitializer)
        {
            _initializer = innerInitializer;
        }

        public void InitializeDatabase(EFTestContext context)
        {
            if(context.Database.Exists()) context.Database.ExecuteSqlCommand("ALTER DATABASE FEEFTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            _initializer.InitializeDatabase(context);
        }
    }
}