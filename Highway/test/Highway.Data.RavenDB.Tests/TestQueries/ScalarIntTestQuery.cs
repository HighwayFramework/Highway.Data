#region

using System.Linq;
using Highway.Data.Tests.TestDomain;

#endregion

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