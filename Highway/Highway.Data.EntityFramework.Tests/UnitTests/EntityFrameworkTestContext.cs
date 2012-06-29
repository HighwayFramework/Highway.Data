using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class EntityFrameworkTestContext : EntityFrameworkContext
    {
        public EntityFrameworkTestContext(string connectionString, IMappingConfiguration configuration) : base(connectionString, configuration)
        {
        }

        public string ConnectionString { get; set; }
    }
}