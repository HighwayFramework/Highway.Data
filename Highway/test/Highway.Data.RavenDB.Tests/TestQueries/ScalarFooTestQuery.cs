#region

using System.Linq;
using Highway.Data.Tests.TestDomain;

#endregion

namespace Highway.Data.RavenDB.Tests.TestQueries
{
    public class ScalarFooTestQuery : Scalar<Foo>
    {
        public ScalarFooTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().First();
        }
    }
}