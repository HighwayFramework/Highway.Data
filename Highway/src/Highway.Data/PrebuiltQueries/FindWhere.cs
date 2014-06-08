using System;
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
        public FindWhere(Expression<Func<T, bool>> whereCondition) {
            ContextQuery = context => context.AsQueryable<T>()
                .Where(whereCondition);
        }

        private FindWhere(Func<IDataContext, IQueryable<T>> contextFilter) {
            ContextQuery = contextFilter;
        }

        /// <summary>
        /// Creates a FindWhere query with results sorted by a given criterion.
        /// </summary>
        /// <typeparam name="TKey">The Type of the item being sorted by</typeparam>
        /// <param name="whereCondition">The filtering condition</param>
        /// <param name="orderBy">The ordering criterion</param>
        /// <example>
        /// FindWhere.OrderedBy(
        ///                     person => person.Name == "Philip", 
        ///                     person => person.DateOfBirth);
        /// </example>
        public static FindWhere<T> OrderedBy<TKey>(
            Expression<Func<T, bool>> whereCondition,
            Expression<Func<T, TKey>> orderBy) {
            Func<IDataContext, IQueryable<T>> contextFilter;
            contextFilter = context => context.AsQueryable<T>()
                .OrderBy(orderBy)
                .Where(whereCondition);
            return new FindWhere<T>(contextFilter);
        }
    }
}
