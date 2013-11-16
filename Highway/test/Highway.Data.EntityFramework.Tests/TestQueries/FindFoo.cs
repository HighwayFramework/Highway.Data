using System.Linq;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.TestQueries
{
    public class FindFoo : Query<Foo>
    {
        public FindFoo()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }

    public class FindFooName : Query<Foo,string>
    {
        public FindFooName()
        {
            Selector = context => context.AsQueryable<Foo>();
            Projector = foos => foos.Select(x => x.Name);
        }
    }
}