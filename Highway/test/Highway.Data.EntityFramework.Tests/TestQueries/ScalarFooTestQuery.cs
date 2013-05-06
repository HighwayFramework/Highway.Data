using System.Linq;
using Highway.Data;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.Tests.TestQueries
{
    public class ScalarFooTestQuery : Scalar<Foo>
    {
        public ScalarFooTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().First();
        }
    }
}