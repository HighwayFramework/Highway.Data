using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    /// <summary>
    /// Allows us to fluently build SecuredRelationships.
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
        /// <param name="pathToSecuredRelationship">An expression representing the path from an instance of T to the property of Type TK that T should be secured by.</param>
        /// <param name="whenNull">The behaviour to use when entitlements are null.</param>
        /// <returns>A Secured Relationship representing how T should be secured by TK.</returns>
        public SecuredRelationship By<TK, TId>(Expression<Func<T, TK>> pathToSecuredRelationship, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            var closuredLambdaBuilder = SecuredPredicateBuilder.BuildPredicateFactoryForSingleProperty<T, TK, TId>(pathToSecuredRelationship, whenNull);
            return new SecuredRelationship(typeof(T), typeof(TK), closuredLambdaBuilder.Compile());
        }

        /// <summary>
        /// Secures an entity by an enumerable property.
        /// </summary>
        /// <typeparam name="TK">The type to secure by.</typeparam>
        /// <typeparam name="TId">The type of the IDs to secure by.</typeparam>
        /// <param name="pathToSecuredCollection">An expression representing the path from an instance of T to the property of Type TK that T should be secured by.</param>
        /// <param name="whenNull">The behaviour to use when entitlements are null.</param>
        /// <returns>A Secured Relationship representing how T should be secured by TK.</returns>
        public SecuredRelationship ByCollection<TK, TId>(Expression<Func<T, IEnumerable<TK>>> pathToSecuredCollection, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            var closuredLambdaBuilder = SecuredPredicateBuilder.BuildPredicateFactoryForCollectionProperty<T, TK, TId>(pathToSecuredCollection, whenNull);
            return new SecuredRelationship(typeof(T), typeof(IEnumerable<TK>), closuredLambdaBuilder.Compile());
        }

        /// <summary>
        /// Secures an entity by its own ID.
        /// </summary>
        /// <typeparam name="TId">The type of the ID.</typeparam>
        /// <returns>A Secured Relationship representing how an entity should secure itself..</returns>
        public SecuredRelationship BySelf<TId>()
        {
            var closuredLambdaBuilder = SecuredPredicateBuilder.BuildPredicateFactoryForSelf<T, TId>();
            return new SecuredRelationship(typeof(T), typeof(T), closuredLambdaBuilder.Compile());
        }
    }
}
