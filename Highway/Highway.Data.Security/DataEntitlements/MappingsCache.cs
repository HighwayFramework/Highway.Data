using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.Extensions;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Security.DataEntitlements
{
    /// <summary>
    /// Caches the <see cref="SecuredRelationship"/> list for different <see cref="ISecuredRelationshipBuilder"/>s so they don't have to be derived each time a caller needs the <see cref="SecuredRelationship"/>s for a given <see cref="Type"/>.
    /// </summary>
    public class MappingsCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingsCache"/> with the given set of <see cref="ISecuredRelationshipBuilder"/>s.
        /// </summary>
        /// <param name="Relationshipses">A collection of <see cref="ISecuredRelationshipBuilder"/>s used to create the cached Relationships</param>
        public MappingsCache(ISecuredRelationshipBuilder[] Relationships)
        {
            _cache = new ConcurrentDictionary<Type, IEnumerable<SecuredRelationship>>();
            var RelationshipGroups = Relationships
                .SelectMany(configuration => configuration.BuildRelationships())
                .DistinctBy(sr => new { sr.Secured, sr.SecuredBy })
                .GroupBy(sr => sr.Secured);

            foreach (var grouping in RelationshipGroups)
            {
                _cache.TryAdd(grouping.Key, grouping);
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/> T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to get <see cref="SecuredRelationship"/>s for.</typeparam>
        /// <returns>The collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/>.</returns>
        public IEnumerable<SecuredRelationship> GetRelationships<T>()
        {
            return GetRelationships(typeof(T));
        }

        /// <summary>
        /// Gets the collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/> entityType.
        /// </summary>
        /// <param name="entityType">The <see cref="Type"/> to get <see cref="SecuredRelationship"/>s for.</param>
        /// <returns>The collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/>.</returns>
        public IEnumerable<SecuredRelationship> GetRelationships(Type entityType)
        {
            IEnumerable<SecuredRelationship> results;
            _cache.TryGetValue(entityType, out results);
            return results;
        }

        /// <summary>
        /// The internal store for the cached Relationships.
        /// </summary>
        private readonly ConcurrentDictionary<Type, IEnumerable<SecuredRelationship>> _cache;
    }
}
