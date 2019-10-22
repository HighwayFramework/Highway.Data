using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.PrebuiltQueries
{
    /// <summary>
    /// Finds all items of a certain type that meet a specified condition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <example>
    /// new FindWhere(person => person.Name == "Philip")
    /// </example>
    public class FindWhere<T> : Query<T> where T : class
    {
        public FindWhere(Expression<Func<T, bool>> whereCondition)
        {
            ContextQuery = context => context.AsQueryable<T>()
                .Where(whereCondition);
        }

        /// <summary>
        /// Sorts the returned results by a specified criteria
        /// </summary>
        /// <typeparam name="TKey">The Type of the item being sorted by</typeparam>
        /// <param name="orderBy">The ordering criterion</param>
        /// <example>
        /// new FindWhere(person => person.Name == "Philip")
        ///              .OrderedBy(person => person.DateOfBirth);
        /// </example>
        public FindWhere<T> OrderedBy<TKey>(Expression<Func<T, TKey>> orderBy)
        {
            var currentContext = ContextQuery;
            ContextQuery = context => currentContext(context).OrderBy(orderBy);
            return this;
        }

        /// <summary>
        /// Sorts the returned results by a specified criteria 
        /// using a particular comparer.
        /// </summary>
        /// <typeparam name="TKey">The Type of the item being sorted by</typeparam>
        /// <param name="orderBy">The ordering criterion</param>
        /// <example>
        /// new FindWhere(person => person.Name == "Philip")
        ///              .OrderedBy(person => person.DateOfBirth);
        /// </example>
        public FindWhere<T> OrderedBy<TKey>(
            Expression<Func<T, TKey>> orderBy,
            IComparer<TKey> comparer)
        {
            var currentContext = ContextQuery;
            ContextQuery = context => currentContext(context)
                .OrderBy(orderBy, comparer);
            return this;
        }
    }
}
