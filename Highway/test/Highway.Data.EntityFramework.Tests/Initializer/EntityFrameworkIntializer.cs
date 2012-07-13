using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class EntityFrameworkIntializer : DropCreateDatabaseAlways<TestContext>
    {
        protected override void Seed(TestContext context)
        {
            for (int i = 0; i < 5;i++ )
            {
                context.Add(new Foo());
            }
            context.SaveChanges();
        }
    }

    public class ForceDeleteInitializer : IDatabaseInitializer<TestContext>
    {
        private readonly IDatabaseInitializer<TestContext> _initializer;

        public ForceDeleteInitializer(IDatabaseInitializer<TestContext> innerInitializer)
        {
            _initializer = innerInitializer;
        }

        public void InitializeDatabase(TestContext context)
        {
            if(context.Database.Exists()) context.Database.ExecuteSqlCommand("ALTER DATABASE FEEFTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            _initializer.InitializeDatabase(context);
        }
    }
}