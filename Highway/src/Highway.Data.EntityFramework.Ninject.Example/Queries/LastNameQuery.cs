using System.Linq;
using Highway.Data.EntityFramework.Ninject.Example.Domain;
using Highway.Data.QueryObjects;

namespace Highway.Data.EntityFramework.Ninject.Example.Queries
{
    public class LastNameQuery : Query<Person>
    {
        public LastNameQuery(string lastName)
        {
            ContextQuery = m => m.AsQueryable<Person>().Where(x => x.LastName == lastName);
        }
    }
}