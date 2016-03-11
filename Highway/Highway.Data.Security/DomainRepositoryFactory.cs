using System;
using System.Linq;
using Highway.Data.Repositories;
using Highway.Data.Security.DataEntitlements;
using Highway.Data.Security.Factories;

namespace Highway.Data.EntityFramework.Security
{
    public class DomainRepositoryFactory : IDomainRepositoryFactory
    {
        public DomainRepositoryFactory(IDomain[] domains, IEntitlementProvider entitlementProviders, MappingsCache mappingsCache)
        {
            _domains = domains;
            _entitlementProviders = entitlementProviders;
            _mappingsCache = mappingsCache;
        }

        public IRepository Create<T>() where T : class, IDomain
        {
            T domain = _domains.OfType<T>().SingleOrDefault();
            return new DomainRepository<T>(new DomainContext<T>(domain, _entitlementProviders, _mappingsCache), domain);
        }

        public IRepository Create(Type type)
        {
            var domain = _domains.FirstOrDefault(x => x.GetType() == type);

            var domainContext = Activator.CreateInstance(typeof(DomainContext<>).MakeGenericType(type), domain, _entitlementProviders, _mappingsCache);

            return (IRepository)Activator.CreateInstance(typeof(DomainRepository<>).MakeGenericType(type), domainContext, domain);
        }

        private readonly IDomain[] _domains;
        private readonly MappingsCache _mappingsCache;
        private readonly IEntitlementProvider _entitlementProviders;
    }
}
