using System.Linq;
using Common.Logging;
using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.EntityFramework.Security
{
    public class DomainContext<T> : Data.DomainContext<T>, IDataContext
        where T : class, IDomain
    {
        public DomainContext(T domain, IEntitlementProvider entitlementProvider, MappingsCache mappingsCache)
            : base(domain)
        {
            _mappingsCache = mappingsCache;
            EntitlementProvider = entitlementProvider;
        }

        public DomainContext(T domain, ILog logger, IEntitlementProvider entitlementProvider)
            : base(domain, logger)
        {
            EntitlementProvider = entitlementProvider;
        }

        public override IQueryable<TEntity> AsQueryable<TEntity>()
        {
            IQueryable<TEntity> query = base.AsQueryable<TEntity>();
            var Maps = _mappingsCache.GetRelationships<TEntity>();
            if (Maps == null)
            {
                return query;
            }
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

        public MappingsCache MappingsCache
        {
            get
            {
                return _mappingsCache;
            }
        }

        public IEntitlementProvider EntitlementProvider { get; private set; }

        private readonly MappingsCache _mappingsCache;
    }
}
