using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Highway.Data.Contexts;
using System.Collections.Generic;

namespace Highway.Data.Tests.InMemory.ActiveDataContext
{
    class Boss : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public List<Minion> Minions { get; set; }
    }

    class Minion : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
    }

    [TestClass]
    public class InMemoryActiveDataContextCollectionTests
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
            interactingDataContext.Add(new Boss { Minions = new List<Minion> { new Minion(), new Minion() } });
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();

            queryingDataContext.AsQueryable<Minion>().Count().Should().Be(2);
        }

        [TestMethod]
        public void ShouldHaveChildItemBeEquivalentNotSame()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var boss = new Boss { FirstName = "Bob", Minions = new List<Minion> { new Minion { FirstName = "Bobby 1" } } };
            interactingDataContext.Add(boss);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedMinion = queryingDataContext.AsQueryable<Minion>().Single();

            queriedMinion.ShouldBeEquivalentTo(boss.Minions.Single());
            queriedMinion.Should().NotBeSameAs(boss.Minions.Single());
        }
    }

    [TestClass]
    public class InMemoryActiveDataContextCollectionTests_GivenInitialData
    {
        [TestInitialize]
        public void Initialize()
        {
            var dataContext = new InMemoryActiveDataContext();
            dataContext = new InMemoryActiveDataContext();
            dataContext.Add(new Boss { FirstName = "Bob", Minions = new List<Minion> { new Minion { FirstName = "Bobby 1" } } });
            dataContext.Commit();
        }

        [TestCleanup]
        public void Cleanup()
        {
            InMemoryActiveDataContext.DropRepository();
        }

        [TestMethod]
        public void ShouldHaveSameChildFromBoss()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var boss = interactingDataContext.AsQueryable<Boss>().Single();
            boss.Minions.Single().FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedMinion = queryingDataContext.AsQueryable<Minion>().Single();
            var queriedBoss = queryingDataContext.AsQueryable<Boss>().Single();

            queriedMinion.Should().BeSameAs(queriedBoss.Minions.Single());
        }

        [TestMethod]
        public void ShouldUpdatePrimitivePropertyFromBoss()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var boss = interactingDataContext.AsQueryable<Boss>().Single();
            boss.Minions.Single().FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedMinion = queryingDataContext.AsQueryable<Minion>().Single();

            queriedMinion.FirstName.Should().Be("Monkey");
        }

        [TestMethod]
        public void ShouldUpdatePrimitivePropertyFromMinion()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var minion = interactingDataContext.AsQueryable<Minion>().Single();
            minion.FirstName = "Monkey";
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            var queriedBoss = queryingDataContext.AsQueryable<Boss>().Single();

            queriedBoss.Minions.Single().FirstName.Should().Be("Monkey");
        }

        [TestMethod]
        public void ShouldRemoveMinion()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var boss = interactingDataContext.AsQueryable<Boss>().Single();
            interactingDataContext.Remove(boss);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Boss>().Count().Should().Be(0);
            queryingDataContext.AsQueryable<Minion>().Count().Should().Be(0);
        }

        [TestMethod]
        public void ShouldNotRemoveBoss()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var minion = interactingDataContext.AsQueryable<Minion>().Single();
            interactingDataContext.Remove(minion);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Boss>().Count().Should().Be(1);
            queryingDataContext.AsQueryable<Minion>().Count().Should().Be(0);
        }

        [TestMethod]
        public void ShouldAddMinion()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var boss = interactingDataContext.AsQueryable<Boss>().Single();
            var dog = new Minion { FirstName = "Dog" };
            boss.Minions.Add(dog);
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Minion>().Count().Should().Be(2);

            var queriedDog = queryingDataContext.AsQueryable<Minion>().Single(m=>m.Id == dog.Id);
            queriedDog.ShouldBeEquivalentTo(dog);
            queriedDog.Should().NotBeSameAs(dog);
        }

        [TestMethod]
        public void ShouldRemoveMinionFromCollection()
        {
            var interactingDataContext = new InMemoryActiveDataContext();
            var boss = interactingDataContext.AsQueryable<Boss>().Single();
            boss.Minions.Clear();
            interactingDataContext.Commit();

            IDataContext queryingDataContext = new InMemoryActiveDataContext();
            queryingDataContext.AsQueryable<Minion>().Count().Should().Be(0);
            queryingDataContext.AsQueryable<Boss>().Single().Minions.Count().Should().Be(0);
        }
    }

}
