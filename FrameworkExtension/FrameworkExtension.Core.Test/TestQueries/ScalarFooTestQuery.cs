using System.Linq;
using FrameworkExtension.Core.QueryObjects;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.Core.Test.TestQueries
{
    public class ScalarFooTestQuery : Scalar<Foo>
    {
        public ScalarFooTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().First();
        }
    }
}