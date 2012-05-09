using System.Linq;
using FrameworkExtension.Core.QueryObjects;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.Core.Test.TestQueries
{
    public class ScalarIntTestQuery : Scalar<int>
    {
        public ScalarIntTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}