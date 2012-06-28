using FrameworkExtension.EntityFramework.Contexts;
using FrameworkExtension.EntityFramework.Mappings;

namespace FrameworkExtension.EntityFramework.Tests.UnitTests
{
    public class EntityFrameworkTestContext : EntityFrameworkContext
    {
        public EntityFrameworkTestContext(string connectionString, IMappingConfiguration configuration) : base(connectionString, configuration)
        {
        }

        public string ConnectionString { get; set; }
    }
}