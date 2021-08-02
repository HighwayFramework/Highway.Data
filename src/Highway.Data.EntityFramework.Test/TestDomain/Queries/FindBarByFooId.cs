using System.Linq;

using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Test.TestDomain.Queries
{
    public class FindBarByFooId : Query<Foo, Bar>
    {
        public FindBarByFooId(int fooId)
        {
            Selector = c => c.AsQueryable<Foo>().Where(foo => foo.Id == fooId);

            Projector = foos => foos.SelectMany(foo => foo.Bars);
        }
    }
}
