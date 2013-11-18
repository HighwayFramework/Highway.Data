#region

using Highway.Data.Tests.TestDomain;

#endregion

namespace Highway.Data.RavenDB.Tests.TestQueries
{
    public class FindFoo : Query<Foo>
    {
        public FindFoo()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}