using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    public class SecuredRelationship
    {
        /// <summary>
        /// Represents a relationship between between two types, one of which is secured by the other.
        /// </summary>
        /// <param name="secured">The secured Type.</param>
        /// <param name="securedBy">The Type by which <paramref name="secured"/> is secured.</param>
        /// <param name="closuredExpressionBuilder">Used to build the predicate by which <paramref name="securedBy"/> secures <paramref name="secured"/>.</param>
        public SecuredRelationship(Type secured, Type securedBy, object closuredExpressionBuilder)
        {
            Secured = secured;
            SecuredBy = securedBy;
            _closuredExpressionBuilder = closuredExpressionBuilder;
        }

        /// <summary>
        /// Restricts a query based on a list of entitledIds.
        /// </summary>
        /// <typeparam name="T">The type of the query to limit.</typeparam>
        /// <typeparam name="TId">The type of the limiting IDs</typeparam>
        /// <param name="query">The query to limit.</param>
        /// <param name="entitledIds">The IDs to limit by.</param>
        /// <returns></returns>
        public IQueryable<T> ApplySecurity<T, TId>(IQueryable<T> query, IEnumerable<TId> entitledIds)
            where T : class
            where TId : IEquatable<TId>
        {
            var typedClosuredExpressionBuilder = (Func<IEnumerable<TId>, Expression<Func<T, bool>>>)_closuredExpressionBuilder;
            var where = typedClosuredExpressionBuilder(entitledIds);
            return query.Where(where);
        }

        /// <summary>
        /// Creates a predicate limiting the Type <typeparamref name="T"/> by the list of <paramref name="entitledIds"/>.
        /// </summary>
        /// <typeparam name="T">The type to limit.</typeparam>
        /// <typeparam name="TId">The type of the limiting IDs</typeparam>
        /// <param name="entitledIds">The list of allowed IDs</param>
        /// <returns>An expression representing a predicate for the Type <typeparamref name="T"/>, limiting that type by the list of <paramref name="entitledIds"/>.</returns>
        public Expression<Func<T, bool>> GetSimplePredicate<T, TId>(IEnumerable<TId> entitledIds)
        {
            var typedClosuredExpressionBuilder = (Func<IEnumerable<TId>, Expression<Func<T, bool>>>)_closuredExpressionBuilder;
            return typedClosuredExpressionBuilder(entitledIds);
        }

        /// <summary>
        /// Creates a predicate limiting the Type <typeparamref name="T"/> by the list of <paramref name="entitledIds"/>.
        /// </summary>
        /// <param name="entitledIds">The list of allowed IDs</param>
        /// <returns>An expression representing a predicate for the Type <typeparamref name="T"/>, limiting that type by the list of <paramref name="entitledIds"/>.</returns>
        public LambdaExpression GetSimplePredicate(IEnumerable<long> entitledIds)
        {
            return (LambdaExpression)((Delegate)_closuredExpressionBuilder).DynamicInvoke(entitledIds);
        }

        /// <summary>
        /// The secured Type.
        /// </summary>
        public Type Secured { get; set; }

        /// <summary>
        /// The Type by which <see cref="SecuredRelationship.Secured"/> is secured.
        /// </summary>
        public Type SecuredBy { get; set; }

        private readonly object _closuredExpressionBuilder;
    }
}
