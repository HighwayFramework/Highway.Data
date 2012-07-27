namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class TestDataContext : Context
    {
        public TestDataContext(string connectionString, IMappingConfiguration mapping) : base(connectionString, mapping)
        {
        }

        public string ConnectionString { get; set; }
    }
}