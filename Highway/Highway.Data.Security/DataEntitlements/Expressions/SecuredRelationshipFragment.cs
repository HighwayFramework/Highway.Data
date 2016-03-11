using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    /// <summary>
    /// Allows us to fluently build Relationships.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SecuredRelationshipFragment<T>
        where T : class
    {
        /// <summary>
        /// Secures an entity by a simple, non-enumerable property.
        /// </summary>
        /// <typeparam name="TK">The type to secure by.</typeparam>
        /// <typeparam name="TId">The type of the IDs to secure by.</typeparam>
        /// <param name="pathToRelationship">An expression representing the path from an instance of T to the property of Type TK that T should be  by.</param>
        /// <param name="whenNull">The behaviour to use when entitlements are null.</param>
        /// <returns>A  Relationship representing how T should be  by TK.</returns>
        public SecuredRelationship By<TK, TId>(Expression<Func<T, TK>> pathToRelationship, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            var closuredLambdaBuilder = PredicateBuilder.BuildPredicateFactoryForSingleProperty<T, TK, TId>(pathToRelationship, whenNull);
            return new SecuredRelationship(typeof(T), typeof(TK), closuredLambdaBuilder.Compile());
        }

        /// <summary>
        /// Secures an entity by an enumerable property.
        /// </summary>
        /// <typeparam name="TK">The type to secure by.</typeparam>
        /// <typeparam name="TId">The type of the IDs to secure by.</typeparam>
        /// <param name="pathToCollection">An expression representing the path from an instance of T to the property of Type TK that T should be  by.</param>
        /// <param name="whenNull">The behaviour to use when entitlements are null.</param>
        /// <returns>A  Relationship representing how T should be  by TK.</returns>
        public SecuredRelationship ByCollection<TK, TId>(Expression<Func<T, IEnumerable<TK>>> pathToCollection, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            var closuredLambdaBuilder = PredicateBuilder.BuildPredicateFactoryForCollectionProperty<T, TK, TId>(pathToCollection, whenNull);
            return new SecuredRelationship(typeof(T), typeof(IEnumerable<TK>), closuredLambdaBuilder.Compile());
        }

        /// <summary>
        /// Secures an entity by its own ID.
        /// </summary>
        /// <typeparam name="TId">The type of the ID.</typeparam>
        /// <returns>A  Relationship representing how an entity should secure itself..</returns>
        public SecuredRelationship BySelf<TId>()
        {
            var closuredLambdaBuilder = PredicateBuilder.BuildPredicateFactoryForSelf<T, TId>();
            return new SecuredRelationship(typeof(T), typeof(T), closuredLambdaBuilder.Compile());
        }
    }
}
