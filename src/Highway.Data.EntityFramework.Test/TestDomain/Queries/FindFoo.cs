
using Highway.Data.Tests.TestDomain;
using System.Linq;


namespace Highway.Data.EntityFramework.Test.TestDomain.Queries
{
    public class FindFoo : Query<Foo>
    {
        public FindFoo()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}