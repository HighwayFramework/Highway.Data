using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.ReadonlyTests.SchoolDomain
{
    public class SchoolDomain : IDomain
    {
        public string ConnectionString => TestConfiguration.Instance.TestDatabaseConnectionString;

        public IContextConfiguration Context { get; }

        public List<IInterceptor> Events { get; }

        public IMappingConfiguration Mappings => new SchoolMapping();
    }
}
