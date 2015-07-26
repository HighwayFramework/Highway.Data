using System.Linq;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.EntityFramework;
using Highway.Data.Tests.TestDomain;
using NUnit.Framework;

namespace Highway.Data.Tests.Async
{
    [TestFixture]
    public class AsyncUsageTests
    {
        [Test]
        public void ShouldExecuteAsyncQuery()
        {
            var inMemoryDataContext = new InMemoryDataContext();
            inMemoryDataContext.Add(new Foo()
            {
                Id = 1
            });
            var repository = new Repository(inMemoryDataContext);

            var task = repository.Find(new TestAsyncQuery());

            task.Result.First().Should().BeAssignableTo<Foo>();
            task.Result.First().Id.Should().Be(1);
        }
    }

    public class TestAsyncQuery : AsyncQuery<Foo>
    {
        public TestAsyncQuery()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}
