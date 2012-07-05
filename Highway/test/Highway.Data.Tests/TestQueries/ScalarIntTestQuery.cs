using System.Linq;
using Highway.Data.QueryObjects;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.Tests.TestQueries
{
    public class ScalarIntTestQuery : Scalar<int>
    {
        public ScalarIntTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}