using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EntityFramework.Security.Configuration;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security
{
    /// <summary>
    ///     Caches the <see cref="Relationship" /> list for different <see cref="IBuildSecuredRelationships" />s so they don't
    ///     have to be derived each time a caller needs the <see cref="Relationship" />s for a given <see cref="Type" />.
    /// </summary>
    public class SecuredRelationshipCache
    {
        private readonly ConcurrentDictionary<Type, IEnumerable<Relationship>> _cache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecuredRelationshipCache" /> with the given set of
        ///     <see cref="IBuildSecuredRelationships" />s.
        /// </summary>
        /// <param name="securedRelationshipBuilders">
        ///     A collection of <see cref="IBuildSecuredRelationships" />s used to create the cached SecuredRelationships
        /// </param>
        public SecuredRelationshipCache(IBuildSecuredRelationships[] securedRelationshipBuilders)
        {
            _cache = new ConcurrentDictionary<Type, IEnumerable<Relationship>>();
            var securedRelationships =
                securedRelationshipBuilders.SelectMany(configuration => configuration.BuildSecuredRelationships())
                    .ToList();
            var groupedConfigurations = securedRelationships.GroupBy(x => x.Secured).ToList();
            foreach (var groupedConfiguration in groupedConfigurations)
            {
                if (groupedConfiguration.Count() > 1)
                {
                    throw new InvalidOperationException(
                        string.Format("Type {0} configured twice, each type must have only one security configuration",
                            groupedConfiguration.Key.Name));
                }
            }

            foreach (var grouping in groupedConfigurations)
            {
                _cache.TryAdd(grouping.Key, grouping.Select(x => x));
            }
        }

        /// <summary>
        ///     Gets the collection of <see cref="Relationship" />s for the given <see cref="Type" /> T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type" /> to get <see cref="Relationship" />s for.</typeparam>
        /// <returns>The collection of <see cref="Relationship" />s for the given <see cref="Type" />.</returns>
        public IEnumerable<Relationship> GetSecuredRelationships<T>()
        {
            return GetSecuredRelationships(typeof (T));
        }

        /// <summary>
        ///     Gets the collection of <see cref="Relationship" />s for the given <see cref="Type" /> entityType.
        /// </summary>
        /// <param name="entityType">
        ///     The <see cref="Type" /> to get <see cref="Relationship" />s for.
        /// </param>
        /// <returns>
        ///     The collection of <see cref="Relationship" />s for the given <see cref="Type" />.
        /// </returns>
        public IEnumerable<Relationship> GetSecuredRelationships(Type entityType)
        {
            IEnumerable<Relationship> results;
            _cache.TryGetValue(entityType, out results);
            return results;
        }
    }
}