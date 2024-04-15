using System;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithIgnoredReadonlyChild
{
    [TestClass]
    public class WhenRemovingParentsWithAReadonlyChild
    {
        private readonly Parent _parent1;

        private readonly Parent _parent2;

        private readonly Repository _repository;

        public WhenRemovingParentsWithAReadonlyChild()
        {
            _parent1 = new Parent { Id = 1, Name = $"{nameof(Parent)}1" };
            _parent2 = new Parent { Id = 2, Name = $"{nameof(Parent)}2" };

            var context = new InMemoryDataContext();
            _repository = new Repository(context);
            _repository.Context.Add(_parent1);
            _repository.Context.Add(_parent2);
            _repository.Context.Commit();
        }

        [TestMethod]
        public void NoExceptionShouldBeThrown()
        {
            Action removeParent = () =>
            {
                _repository.Context.Remove(_parent1);
                _repository.Context.Remove(_parent2);
                _repository.Context.Commit();
            };

            removeParent.Should().NotThrow();
        }
    }
}
