using Highway.Data;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.NHibernate.Tests
{
    public class FindFoo : Query<Foo>
    {
        public FindFoo()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}