using System;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Test.InMemory.BugTests.StaticReadonlyProperties
{
    [TestClass]
    public class WhenStaticReadonlyCollectionPropertiesArePresent
    {
        [TestMethod]
        public void TheirObjectsCanBeRemovedWithoutError()
        {
            // Arrange
            var baz1 = new Baz { Id = 1, Name = $"{nameof(Baz)}1" };
            var baz2 = new Baz { Id = 2, Name = $"{nameof(Baz)}2" };
            var quxes1 = baz1.BazQuxes;
            var quxes2 = baz2.BazQuxes;

            var context = new InMemoryDataContext();
            var repository = new Repository(context);
            repository.Context.Add(baz1);
            repository.Context.Add(baz2);
            Action removeBaz = () =>
            {
                repository.Context.Remove(baz2);
                repository.Context.Commit();
            };

            // Act
            repository.Context.Commit();

            // Assert
            quxes1.Should().BeSameAs(quxes2);
            removeBaz.Should().NotThrow();
        }
    }
}
