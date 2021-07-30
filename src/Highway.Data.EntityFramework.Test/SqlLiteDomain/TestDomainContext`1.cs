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
    }
}
