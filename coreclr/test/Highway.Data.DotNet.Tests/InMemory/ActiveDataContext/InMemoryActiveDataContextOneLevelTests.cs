using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Highway.Data.InMemory;

namespace Highway.Data.DotNet.Tests.InMemory.ActiveDataContext
{
    class Person : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
    }

    [TestFixture]
    public class InMemoryActiveDataContextOneLevelTests
    {
        [TearDown]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }


        [Test]
        public void ShouldHaveNoItems()
        {
            var interactingDataContext = new InMemoryActiveDataContext();

            interactingDataContext.Add(new Person());

            IDataContext queryingDataContext = new InMemoryActiveDataContext();

            queryingDataContext.AsQueryable<Person>().Count().Should().Be(0);
        }

        [Test]
        public void ShouldHaveItemCommitted()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            interactingDataContext.Add(new Person());
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();

            queryingDataContext.AsQueryable<Person>().Count().Should().Be(1);
        }

        [Test]
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

    [TestFixture]
    public class InMemoryActiveDataContextOneLevelTests_GivenInitialData
    {
        [SetUp]
        public void Initialize()
        {
            var dataContext = new InMemoryActiveDataContext();
            dataContext = new InMemoryActiveDataContext();
            dataContext.Add(new Person { FirstName = "Bob" });
            dataContext.Commit();
        }

        [TearDown]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }

        [Test]
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

        [Test]
        public void ShouldNotRemoveItem()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var person = interactingDataContext.AsQueryable<Person>().Single();
            interactingDataContext.Remove(person);

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Person>().Count().Should().Be(1);
        }

        [Test]
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
