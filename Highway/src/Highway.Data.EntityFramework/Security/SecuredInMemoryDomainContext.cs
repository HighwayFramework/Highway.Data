using System.Linq;
using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.EntityFramework.Security
{
    public class SecuredInMemoryDomainContext<T> : Contexts.InMemoryDomainContext<T>
        where T : class
    {
        public SecuredInMemoryDomainContext(IEntitlementProvider entitlementProvider, SecuredMappingsCache mappingsCache)
        {
            _mappingsCache = mappingsCache;
            EntitlementProvider = entitlementProvider;
        }

        public override IQueryable<TEntity> AsQueryable<TEntity>()
        {
            var securedMaps = _mappingsCache.GetSecuredRelationships<TEntity>();
            IQueryable<TEntity> query = base.AsQueryable<TEntity>();
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

        public IEntitlementProvider EntitlementProvider { get; private set; }

        private readonly SecuredMappingsCache _mappingsCache;
    }
}
