using System.Linq;
using Highway.Data.GettingStarted.Domain.Entities;
using Highway.Data.QueryObjects;

namespace Highway.Data.GettingStarted.AdvancedQueries
{
    public class FindPersonByLastNameEmbeddedSQLQuery : AdvancedQuery<Person>
    {
        public FindPersonByLastNameEmbeddedSQLQuery(string lastName)
        {
            ContextQuery = delegate(DataContext context)
                {
                    var sql = "Select * from People p where p.LastName = @last";
                    var results = context.Database.SqlQuery<Person>(sql, lastName);
                    return results.AsQueryable();
                };
        }
    }
}
