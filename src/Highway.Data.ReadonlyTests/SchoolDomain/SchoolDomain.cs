using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.ReadonlyTests
{
    public class SchoolDomain : IDomain
    {
        public string ConnectionString { get; } = Configuration.Instance.TestDatabaseConnectionString;

        public IContextConfiguration Context { get; } = new DefaultContextConfiguration();

        public List<IInterceptor> Events { get; } = new();

        public IMappingConfiguration Mappings { get; } = new SchoolMapping();
    }
}
