using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class ForceDeleteInitializer : IDatabaseInitializer<TestDataContext>
    {
        private readonly IDatabaseInitializer<TestDataContext> _initializer;

        public ForceDeleteInitializer(IDatabaseInitializer<TestDataContext> innerInitializer)
        {
            _initializer = innerInitializer;
        }

        public void InitializeDatabase(TestDataContext context)
        {
            if (context.Database.Exists()) context.Database.ExecuteSqlCommand("ALTER DATABASE FEEFTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            _initializer.InitializeDatabase(context);
        }
    }
}
