using System;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Test.InMemory.BugTests.StaticReadonlyProperties
{
    [TestClass]
    public class WhenStaticReadonlyPropertiesArePresent
    {
        [TestMethod]
        public void TheirObjectsCanBeRemovedWithoutError()
        {
            // Arrange
            var foo1 = new Foo { Id = 1, Name = $"{nameof(Foo)}1" };
            var foo2 = new Foo { Id = 2, Name = $"{nameof(Foo)}2" };
            var bar1 = foo1.FooBar;
            var bar2 = foo2.FooBar;

            var context = new InMemoryDataContext();
            var repository = new Repository(context);
            repository.Context.Add(foo1);
            repository.Context.Add(foo2);
            Action removeFoo = () =>
            {
                repository.Context.Remove(foo1);
                repository.Context.Commit();
            };

            // Act
            repository.Context.Commit();

            // Assert
            bar1.Should().Be(bar2);
            removeFoo.Should().NotThrow();
        }
    }
}
