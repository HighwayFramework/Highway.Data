using System.Collections.Generic;
using System.Linq;

using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.ReadonlyTests
{
    public class SchoolDomain : IDomain
    {
        public SchoolDomain(IEnumerable<IInterceptor> events = null)
        {
            Events = events?.ToList();
        }

        public string ConnectionString { get; } = Configuration.Instance.TestDatabaseConnectionString;

        public IContextConfiguration Context { get; } = new DefaultContextConfiguration();

        public List<IInterceptor> Events { get; }

        public IMappingConfiguration Mappings { get; } = new SchoolMapping();
    }
}
