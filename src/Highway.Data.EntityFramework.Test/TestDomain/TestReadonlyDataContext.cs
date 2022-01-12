using Common.Logging;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class TestReadonlyDataContext : ReadonlyDataContext
    {
        public TestReadonlyDataContext(string connectionString, ILog logger)
            : base(connectionString, null, logger)
        {
        }

        public string ConnectionString { get; set; }
    }
}
