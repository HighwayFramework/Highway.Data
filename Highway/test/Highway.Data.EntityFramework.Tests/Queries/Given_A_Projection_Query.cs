using System.Linq;
using Highway.Data;
using Highway.Data.Tests.TestDomain;
using Highway.Data.Tests.TestQueries;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class Given_A_Projection_Query
    {
        public Given_A_Projection_Query()
        {
        }
    }

    public class ExampleProjection : Query<Foo,string>
    {
        public ExampleProjection()
        {
            Selector = context => context.AsQueryable<Foo>();
            Projector = objects => objects.Select(x => x.Name);
        }
    }
}