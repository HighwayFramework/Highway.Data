using System.Data.Entity;

using Devart.Data.SQLite.Entity;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain
{
    [DbConfigurationType(typeof(SQLiteEntityProviderServicesConfiguration))]
    public class TestDomainContext<T> : DomainContext<T>
        where T : class, IDomain
    {
        /// <summary>
        ///     Constructs the domain context
        /// </summary>
        /// <param name="domain"></param>
        public TestDomainContext(T domain)
            : base(domain)
        {
        }
    }
}
