using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class IncludeGraphExtensions
    {
        [TestMethod]
        public void ShouldAllowMultipleLevelIncludes()
        {
            //arrange 
            var fooQuery = new FindFooWithGraph();

            //act

            //assert

        }
    }

    public class FindFooWithGraph : Query<Foo>
    {
        public FindFooWithGraph()
        {
            ContextQuery = c => c.AsQueryable<Foo>().IncludeMany(x=>x.Bars).ThenInclude(x=>x.Qux);
        }
    }
}