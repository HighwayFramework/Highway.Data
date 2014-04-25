using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Highway.Data.Contexts;

namespace Highway.Data.Tests.InMemory.ActiveDataContext
{
    class Parent : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public Child Child { get; set; }
    }

    class Child : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
    }

    [TestClass]
    public class InMemoryActiveDataContextTwoLevelTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }


        [TestMethod]
        public void ShouldHaveItemsWhenCommitted()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            interactingDataContext.Add(new Parent { Child = new Child() });
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();

            queryingDataContext.AsQueryable<Child>().Count().Should().Be(1);
        }

        [TestMethod]
        public void ShouldHaveChildItemBeEquivalentNotSame()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var parent = new Parent { FirstName = "Bob", Child = new Child { FirstName="Bobby" } };
            interactingDataContext.Add(parent);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedChild = queryingDataContext.AsQueryable<Child>().Single();

            queriedChild.ShouldBeEquivalentTo(parent.Child);
            queriedChild.Should().NotBeSameAs(parent.Child);
        }
    }

    [TestClass]
    public class InMemoryActiveDataContextTwoLevelTests_GivenInitialData
    {
        [TestInitialize]
        public void Initialize()
        {
            var dataContext = new InMemoryActiveDataContext();
            dataContext = new InMemoryActiveDataContext();
            dataContext.Add(new Parent { FirstName = "Bob", Child = new Child { FirstName = "Bobby" } });
            dataContext.Commit();
        }

        [TestCleanup]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }

        [TestMethod]
        public void ShouldHaveSameChildFromParent()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var parent = interactingDataContext.AsQueryable<Parent>().Single();
            parent.Child.FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedChild = queryingDataContext.AsQueryable<Child>().Single();
            var queriedParent = queryingDataContext.AsQueryable<Parent>().Single();

            queriedChild.Should().BeSameAs(queriedParent.Child);
        }

        [TestMethod]
        public void ShouldUpdatePrimitivePropertyFromParent()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var parent = interactingDataContext.AsQueryable<Parent>().Single();
            parent.Child.FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedChild = queryingDataContext.AsQueryable<Child>().Single();

            queriedChild.FirstName.Should().Be("Monkey");
        }

        [TestMethod]
        public void ShouldUpdatePrimitivePropertyFromChild()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var child = interactingDataContext.AsQueryable<Child>().Single();
            child.FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedParent = queryingDataContext.AsQueryable<Parent>().Single();

            queriedParent.Child.FirstName.Should().Be("Monkey");
        }
        
        [TestMethod]
        public void ShouldRemoveChild()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var parent = interactingDataContext.AsQueryable<Parent>().Single();
            interactingDataContext.Remove(parent);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Parent>().Count().Should().Be(0);
            queryingDataContext.AsQueryable<Child>().Count().Should().Be(0);
        }

        [TestMethod]
        public void ShouldNotRemoveParent()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var child = interactingDataContext.AsQueryable<Child>().Single();
            interactingDataContext.Remove(child);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Parent>().Count().Should().Be(1);
            queryingDataContext.AsQueryable<Child>().Count().Should().Be(0);
        }
    }

}
