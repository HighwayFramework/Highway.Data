
using Highway.Data.Filtering;


namespace Highway.Data
{
    /// <summary>
    ///     A Query object that takes Criteria and then returns the filtered IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindByCriteria<T> : Query<T> where T : class
    {
        /// <summary>
        ///     Constructs the query
        /// </summary>
        /// <param name="criteria">The criteria that filters the query</param>
        public FindByCriteria(Criteria criteria)
        {
            ContextQuery = context => context.AsQueryable<T>().FilterBy(criteria);
        }
    }
}