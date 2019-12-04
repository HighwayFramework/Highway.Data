using Common.Logging;
using System;
using System.Data.Entity;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain
{
    [DbConfigurationType(typeof(Devart.Data.SQLite.Entity.SQLiteEntityProviderServicesConfiguration))]

    public class TestDomainContext<T> : DomainContext<T> where T : class, IDomain
    {
        /// <summary>
        /// Constructs the domain context
        /// </summary>
        /// <param name="domain"></param>
        public TestDomainContext(T domain) : base(domain)
        {

        }

        /// <summary>
        /// Constructs the domain context
        /// </summary>
        /// <param name="domain">domain for context</param>
        /// <param name="logger">logger</param>
        public TestDomainContext(T domain, ILog logger) : base(domain, logger)
        {

        }
    }
}
