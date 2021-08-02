using System.Linq;

using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Test.TestDomain.Queries
{
    public class FindFooName : Query<Foo, string>
    {
        public FindFooName()
        {
            Selector = context => context.AsQueryable<Foo>();
            Projector = foos => foos.Select(x => x.Name);
        }
    }
}
