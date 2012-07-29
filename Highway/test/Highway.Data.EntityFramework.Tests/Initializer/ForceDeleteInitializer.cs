using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class ForceDeleteInitializer : IDatabaseInitializer<TestDataDataContext>
    {
        private readonly IDatabaseInitializer<TestDataDataContext> _initializer;

        public ForceDeleteInitializer(IDatabaseInitializer<TestDataDataContext> innerInitializer)
        {
            _initializer = innerInitializer;
        }

        public void InitializeDatabase(TestDataDataContext dataDataContext)
        {
            if (dataDataContext.Database.Exists()) dataDataContext.Database.ExecuteSqlCommand("ALTER DATABASE FEEFTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            _initializer.InitializeDatabase(dataDataContext);
        }
    }
}
