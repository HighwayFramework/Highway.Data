using System.Linq;
using Highway.Data.Contexts;
using Highway.Data.Tests.TestDomain;
using NUnit.Framework;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestFixture]
    public class MultipleCommitsOrphansItems
    {
        [Test]
        public void ShouldAllowForQueryingContext()
        {
            var repository = new Repository(new InMemoryDataContext());

            repository.Context.Add(new Foo());
            repository.Context.Commit();

            var foo = repository.Find(new FindFooById(1));

            Assert.IsNotNull(foo);
        }
    }

    public class FindFooById : Scalar<object>
    {
        public FindFooById(int i)
        {
            ContextQuery = c => c.AsQueryable<Foo>().SingleOrDefault(x => x.Id == i);
        }
    }
}