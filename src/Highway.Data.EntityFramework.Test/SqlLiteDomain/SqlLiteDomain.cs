using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain
{
    public class SqlLiteDomain : IDomain
    {
        public string ConnectionString => @"Data Source=:memory:";

        public IMappingConfiguration Mappings => new SqlLiteDomainMappings();

        public IContextConfiguration Context => new SqlLiteDomainContextConfiguration();

        public List<IInterceptor> Events => new List<IInterceptor>
        {
        };
    }
}
