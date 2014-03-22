#region

using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class LongIdentityStrategyTests
    {
        private readonly long seedNumber = 500;
        private LongIdentityStrategy<Entity> target;

        [TestInitialize]
        public void Setup()
        {
            LongIdentityStrategy<Entity>.LastValue = seedNumber;
            target = new LongIdentityStrategy<Entity>(x => x.Id);
        }

        [TestMethod]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            var result = target.Next();

            // Assert
            result.Should().Be(seedNumber + 1);
        }

        [TestMethod]
        public void Assign_ShouldAssignId()
        {
            // Arrange
            var entity = new Entity {Id = 0};

            // Act
            target.Assign(entity);

            // Assert
            entity.Id.Should().Be(seedNumber + 1);
        }

        class Entity
        {
            public long Id { get; set; }
        }
    }
}