using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithReadonlyChildren
{
    [TestClass]
    public class WhenAddingParentsWithReadonlyChildren
    {
        private readonly Repository _repository;

        public WhenAddingParentsWithReadonlyChildren()
        {
            var parent1 = new Parent { Id = 1, Name = $"{nameof(Parent)}1" };
            var parent2 = new Parent { Id = 2, Name = $"{nameof(Parent)}2" };

            var context = new InMemoryDataContext();
            _repository = new Repository(context);
            _repository.Context.Add(parent1);
            _repository.Context.Add(parent2);
            _repository.Context.Commit();
        }

        [TestMethod]
        public void TheContextShouldContainTwoChildren()
        {
            var query = new FindAll<Child>();
            _repository.Find(query).Should().HaveCount(2);
        }

        [TestMethod]
        public void TheContextShouldContainTwoParents()
        {
            var query = new FindAll<Parent>();
            _repository.Find(query).Should().HaveCount(2);
        }
    }
}
