using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Highway.Data.Contexts;

namespace Highway.Data.Tests.InMemory.ActiveDataContext
{
    class Person : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
    }

    [TestClass]
    public class InMemoryActiveDataContextOneLevelTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }


        [TestMethod]
        public void ShouldHaveNoItems()
        {
            var interactingDataContext = new InMemoryActiveDataContext();

            interactingDataContext.Add(new Person());

            IDataContext queryingDataContext = new InMemoryActiveDataContext();

            queryingDataContext.AsQueryable<Person>().Count().Should().Be(0);
        }

        [TestMethod]
        public void ShouldHaveItemCommitted()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            interactingDataContext.Add(new Person());
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();

            queryingDataContext.AsQueryable<Person>().Count().Should().Be(1);
        }

        [TestMethod]
        public void ShouldHaveItemBeEquivalentNotSame()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var originalPerson = new Person { FirstName = "Bob" };
            interactingDataContext.Add(originalPerson);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedPerson = queryingDataContext.AsQueryable<Person>().Single();

            queriedPerson.ShouldBeEquivalentTo(originalPerson);
            queriedPerson.Should().NotBeSameAs(originalPerson);
        }
    }

    [TestClass]
    public class InMemoryActiveDataContextOneLevelTests_GivenInitialData
    {
        [TestInitialize]
        public void Initialize()
        {
            var dataContext = new InMemoryActiveDataContext();
            dataContext = new InMemoryActiveDataContext();
            dataContext.Add(new Person { FirstName = "Bob" });
            dataContext.Commit();
        }

        [TestCleanup]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }

        [TestMethod]
        public void ShouldUpdatePrimitiveProperty()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var person = interactingDataContext.AsQueryable<Person>().Single();
            person.FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Person>();

            queryingDataContext.AsQueryable<Person>().Single().FirstName.Should().Be("Monkey");
        }

        [TestMethod]
        public void ShouldNotRemoveItem()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var person = interactingDataContext.AsQueryable<Person>().Single();
            interactingDataContext.Remove(person);

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Person>().Count().Should().Be(1);
        }

        [TestMethod]
        public void ShouldRemoveItem()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var person = interactingDataContext.AsQueryable<Person>().Single();
            interactingDataContext.Remove(person);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Person>().Count().Should().Be(0);
        }
    }

}
