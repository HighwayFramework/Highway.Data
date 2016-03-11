using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class LongIdentityStrategyTests
    {
        private readonly long seedNumber = 500;
        private LongIdentityStrategy<Entity> target;

        [SetUp]
        public void Setup()
        {
            LongIdentityStrategy<Entity>.LastValue = seedNumber;
            target = new LongIdentityStrategy<Entity>(x => x.Id);
        }

        [Test]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            var result = target.Next();

            // Assert
            result.Should().Be(seedNumber + 1);
        }

        [Test]
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
