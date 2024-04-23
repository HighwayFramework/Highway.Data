using System.Linq;

using Highway.Data.Tests.TestDomain;

namespace Highway.Data.Tests.InMemory.BugTests
{
    public class FindFooById : Scalar<object>
    {
        public FindFooById(int i)
        {
            ContextQuery = c => c.AsQueryable<Foo>().SingleOrDefault(x => x.Id == i);
        }
    }
}