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
    public class SecuredMappingsCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredMappingsCache"/> with the given set of <see cref="ISecuredRelationshipBuilder"/>s.
        /// </summary>
        /// <param name="securedRelationshipses">A collection of <see cref="ISecuredRelationshipBuilder"/>s used to create the cached SecuredRelationships</param>
        public SecuredMappingsCache(ISecuredRelationshipBuilder[] securedRelationshipses)
        {
            _cache = new ConcurrentDictionary<Type, IEnumerable<SecuredRelationship>>();
            var securedRelationshipGroups = securedRelationshipses
                .SelectMany(configuration => configuration.BuildSecuredRelationships())
                .DistinctBy(sr => new { sr.Secured, sr.SecuredBy })
                .GroupBy(sr => sr.Secured);

            foreach (var grouping in securedRelationshipGroups)
            {
                _cache.TryAdd(grouping.Key, grouping);
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/> T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to get <see cref="SecuredRelationship"/>s for.</typeparam>
        /// <returns>The collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/>.</returns>
        public IEnumerable<SecuredRelationship> GetSecuredRelationships<T>()
        {
            return GetSecuredRelationships(typeof(T));
        }

        /// <summary>
        /// Gets the collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/> entityType.
        /// </summary>
        /// <param name="entityType">The <see cref="Type"/> to get <see cref="SecuredRelationship"/>s for.</param>
        /// <returns>The collection of <see cref="SecuredRelationship"/>s for the given <see cref="Type"/>.</returns>
        public IEnumerable<SecuredRelationship> GetSecuredRelationships(Type entityType)
        {
            IEnumerable<SecuredRelationship> results;
            _cache.TryGetValue(entityType, out results);
            return results;
        }

        /// <summary>
        /// The internal store for the cached SecuredRelationships.
        /// </summary>
        private readonly ConcurrentDictionary<Type, IEnumerable<SecuredRelationship>> _cache;
    }
}
