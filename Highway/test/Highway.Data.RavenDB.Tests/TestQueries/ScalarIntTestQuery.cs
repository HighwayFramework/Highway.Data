using System.Linq;
using Highway.Data;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.RavenDB.Tests.TestQueries
{
    public class ScalarIntTestQuery : Scalar<int>
    {
        public ScalarIntTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}