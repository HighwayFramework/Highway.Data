using System;
using System.Linq;
using Highway.Data.Repositories;
using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.EntityFramework.Security
{
    public class SecuredDomainRepositoryFactory : ISecuredDomainRepositoryFactory
    {
        public SecuredDomainRepositoryFactory(IDomain[] domains, IEntitlementProvider entitlementProviders, SecuredMappingsCache mappingsCache)
        {
            _domains = domains;
            _entitlementProviders = entitlementProviders;
            _mappingsCache = mappingsCache;
        }

        public IRepository Create<T>() where T : class, IDomain
        {
            T domain = _domains.OfType<T>().SingleOrDefault();
            return new DomainRepository<T>(new SecuredDomainContext<T>(domain, _entitlementProviders, _mappingsCache), domain);
        }

        public IRepository Create(Type type)
        {
            var domain = _domains.FirstOrDefault(x => x.GetType() == type);

            var domainContext = Activator.CreateInstance(typeof(SecuredDomainContext<>).MakeGenericType(type), domain, _entitlementProviders, _mappingsCache);

            return (IRepository)Activator.CreateInstance(typeof(DomainRepository<>).MakeGenericType(type), domainContext, domain);
        }

        private readonly IDomain[] _domains;
        private readonly SecuredMappingsCache _mappingsCache;
        private readonly IEntitlementProvider _entitlementProviders;
    }
}
