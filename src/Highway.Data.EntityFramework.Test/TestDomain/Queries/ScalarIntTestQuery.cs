using System.Linq;

using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Test.TestDomain.Queries
{
    public class ScalarIntTestQuery : Scalar<int>
    {
        public ScalarIntTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}
