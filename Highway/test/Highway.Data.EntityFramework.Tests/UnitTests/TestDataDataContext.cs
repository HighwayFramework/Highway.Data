namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class TestDataDataContext : DataContext
    {
        public TestDataDataContext(string connectionString, IMappingConfiguration mapping) : base(connectionString, mapping)
        {
        }

        public string ConnectionString { get; set; }
    }
}