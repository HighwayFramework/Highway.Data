using System.Linq;
using Highway.Data.QueryObjects;
using Highway.Data.Test.TestDomain;

namespace Highway.Data.Test.TestQueries
{
    public class ScalarFooTestQuery : Scalar<Foo>
    {
        public ScalarFooTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().First();
        }
    }
}