using System.Linq;
using Highway.Data.QueryObjects;
using Highway.Data.Test.TestDomain;

namespace Highway.Data.Test.TestQueries
{
    public class ScalarIntTestQuery : Scalar<int>
    {
        public ScalarIntTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}