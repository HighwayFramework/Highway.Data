using Highway.Data.QueryObjects;
using Highway.Data.Test.TestDomain;

namespace Highway.Data.Test.TestQueries
{
    public class FindFoo : Query<Foo>
    {
        public FindFoo()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}