using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;

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
