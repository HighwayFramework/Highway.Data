using FluentAssertions;

using Highway.Data.Contexts;
using Highway.Data.Tests.TestDomain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class TestMultipleCommitsOrphansItems
    {
        [TestMethod]
        public void ShouldAllowForQueryingContext()
        {
            var repository = new Repository(new InMemoryDataContext());

            repository.Context.Add(new Foo());
            repository.Context.Commit();

            var foo = repository.Find(new FindFooById(1));

            foo.Should().NotBeNull();
        }
    }
}
