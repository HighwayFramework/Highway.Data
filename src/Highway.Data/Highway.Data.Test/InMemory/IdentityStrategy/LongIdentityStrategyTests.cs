using FluentAssertions;
using Highway.Data.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Highway.Data.Tests.InMemory
{

    [TestClass]
    public class LongIdentityStrategyTests
    {
        private LongIdentityStrategy<Entity> target;

        [TestInitialize]
        public void Setup()
        {
            target = new LongIdentityStrategy<Entity>(x => x.Id);
        }

        [TestMethod]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            var result = target.Next();

            // Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void Assign_ShouldAssignId()
        {
            // Arrange
            var entity = new Entity { Id = 0 };

            // Act
            target.Assign(entity);

            // Assert
            entity.Id.Should().Be(1);
        }

        class Entity
        {
            public long Id { get; set; }
        }
    }
}