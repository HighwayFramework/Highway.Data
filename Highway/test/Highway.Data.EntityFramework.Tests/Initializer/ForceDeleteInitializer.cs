using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class ForceDeleteInitializer : IDatabaseInitializer<TestContext>
    {
        private readonly IDatabaseInitializer<TestContext> _initializer;

        public ForceDeleteInitializer(IDatabaseInitializer<TestContext> innerInitializer)
        {
            _initializer = innerInitializer;
        }

        public void InitializeDatabase(TestContext context)
        {
            if (context.Database.Exists()) context.Database.ExecuteSqlCommand("ALTER DATABASE FEEFTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            _initializer.InitializeDatabase(context);
        }
    }
}
