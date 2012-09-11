using Common.Logging;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class TestDataContext : DataContext
    {
        public TestDataContext(string connectionString, IMappingConfiguration mapping, ILog logger)
            : base(connectionString, mapping, null, logger)
        {
        }

        public string ConnectionString { get; set; }
    }
}