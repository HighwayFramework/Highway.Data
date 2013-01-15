using Highway.Data.Filtering;
using Highway.Data.QueryObjects;

namespace Highway.Data.GettingStarted.DataAccess.Queries
{
    public class FindByCriteria<T> : Query<T> where T : class
    {
        public FindByCriteria(Criteria criteria)
        {
            ContextQuery = context => context.AsQueryable<T>().FilterBy(criteria);
        }
    }
}