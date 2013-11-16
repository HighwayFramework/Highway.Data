using System.Linq;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.TestQueries
{
    public class ScalarFooTestQuery : Scalar<Foo>
    {
        public ScalarFooTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().First();
        }
    }
}