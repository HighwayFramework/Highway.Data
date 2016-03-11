using System.Linq;
using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.EntityFramework.Security
{
    public class InMemoryDomainContext<T> : Contexts.InMemoryDomainContext<T>
        where T : class
    {
        public InMemoryDomainContext(IEntitlementProvider entitlementProvider, MappingsCache mappingsCache)
        {
            _mappingsCache = mappingsCache;
            EntitlementProvider = entitlementProvider;
        }

        public override IQueryable<TEntity> AsQueryable<TEntity>()
        {
            var Maps = _mappingsCache.GetRelationships<TEntity>();
            IQueryable<TEntity> query = base.AsQueryable<TEntity>();
            foreach (var Relationship in Maps)
            {
                var singleType = Relationship.SecuredBy.ToSingleType();
                if (!EntitlementProvider.IsEntitledToAll(singleType))
                {
                    var entitledIds = EntitlementProvider.GetEntitledIds(singleType);
                    if (entitledIds != null && entitledIds.Any())
                    {
                        query = Relationship.ApplySecurity(query, entitledIds);
                    }
                }
            }
            return query;
        }

        public IEntitlementProvider EntitlementProvider { get; private set; }

        private readonly MappingsCache _mappingsCache;
    }
}
