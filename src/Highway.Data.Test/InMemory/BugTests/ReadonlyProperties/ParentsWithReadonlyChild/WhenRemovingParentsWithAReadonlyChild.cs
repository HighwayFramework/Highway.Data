using System;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithReadonlyChild
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
        public void TheCorrectExceptionShouldBeThrown()
        {
            Action removeParent = () =>
            {
                _repository.Context.Remove(_parent1);
                _repository.Context.Remove(_parent2);
                _repository.Context.Commit();
            };

            removeParent
                .Should()
                .Throw<ArgumentException>()
                .WithMessage($"An entry could not be removed from the {nameof(InMemoryDataContext)} because its referencing property has no setter.  The entry"
                             + $" was scheduled for removal because it is referenced from {typeof(ParentBase).FullName} through the property"
                             + $" {nameof(ParentBase)}.{nameof(_parent1.Child)}.  Either add a setter to this property, or decorate it with the"
                             + $" {nameof(InMemoryIgnoreAttribute)}.");
        }
    }
}
