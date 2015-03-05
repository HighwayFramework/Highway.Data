using System.Linq;
using Common.Logging;
using Highway.Data.Security;
using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.EntityFramework.Security
{
    public class SecuredDomainContext<T> : DomainContext<T>, ISecuredDataContext
        where T : class, IDomain
    {
        public SecuredDomainContext(T domain, IEntitlementProvider entitlementProvider, SecuredMappingsCache mappingsCache)
            : base(domain)
        {
            _mappingsCache = mappingsCache;
            EntitlementProvider = entitlementProvider;
        }

        public SecuredDomainContext(T domain, ILog logger, IEntitlementProvider entitlementProvider)
            : base(domain, logger)
        {
            EntitlementProvider = entitlementProvider;
        }

        public override IQueryable<TEntity> AsQueryable<TEntity>()
        {
            IQueryable<TEntity> query = base.AsQueryable<TEntity>();
            var securedMaps = _mappingsCache.GetSecuredRelationships<TEntity>();
            if (securedMaps == null)
            {
                return query;
            }
            foreach (var securedRelationship in securedMaps)
            {
                var singleType = securedRelationship.SecuredBy.ToSingleType();
                if (!EntitlementProvider.IsEntitledToAll(singleType))
                {
                    var entitledIds = EntitlementProvider.GetEntitledIds(singleType);
                    if (entitledIds != null && entitledIds.Any())
                    {
                        query = securedRelationship.ApplySecurity(query, entitledIds);
                    }
                }
            }
            return query;
        }

        public SecuredMappingsCache MappingsCache
        {
            get
            {
                return _mappingsCache;
            }
        }

        public IEntitlementProvider EntitlementProvider { get; private set; }

        private readonly SecuredMappingsCache _mappingsCache;
    }
}
