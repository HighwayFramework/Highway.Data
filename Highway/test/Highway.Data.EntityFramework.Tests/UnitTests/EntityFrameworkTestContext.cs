using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;

namespace Highway.Data.NHibernate.Tests.UnitTests
{
    public class TestContext : Context
    {
        public TestContext(string connectionString, IMappingConfiguration[] configurations) : base(connectionString, configurations)
        {
        }

        public string ConnectionString { get; set; }
    }
}