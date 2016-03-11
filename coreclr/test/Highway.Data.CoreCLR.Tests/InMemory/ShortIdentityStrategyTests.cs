using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class ShortIdentityStrategyTests
    {
        private readonly short seedNumber = 500;
        private ShortIdentityStrategy<Entity> target;

        [SetUp]
        public void Setup()
        {
            ShortIdentityStrategy<Entity>.LastValue = seedNumber;
            target = new ShortIdentityStrategy<Entity>(x => x.Id);
        }

        [Test]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            var result = target.Next();

            // Assert
            Assert.AreEqual(seedNumber + 1, result);
        }

        [Test]
        public void Assign_ShouldAssignId()
        {
            // Arrange
            var entity = new Entity {Id = 0};

            // Act
            target.Assign(entity);

            // Assert
            Assert.AreEqual(seedNumber + 1, entity.Id);
        }

        class Entity
        {
            public short Id { get; set; }
        }
    }
}
