using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class LongIdentityStrategyTests
    {
        private LongIdentityStrategy<Entity> _target;

        [TestMethod]
        public void Assign_ShouldAssignId()
        {
            // Arrange
            var entity = new Entity { Id = 0 };

            // Act
            _target.Assign(entity);

            // Assert
            entity.Id.Should().Be(1);
        }

        [TestMethod]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            var result = _target.Next();

            // Assert
            result.Should().Be(1);
        }

        [TestInitialize]
        public void Setup()
        {
            _target = new LongIdentityStrategy<Entity>(x => x.Id);
        }

        private class Entity
        {
            public long Id { get; set; }
        }
    }
}
