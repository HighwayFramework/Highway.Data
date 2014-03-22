#region

using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class ShortIdentityStrategyTests
    {
        private readonly short seedNumber = 500;
        private ShortIdentityStrategy<Entity> target;

        [TestInitialize]
        public void Setup()
        {
            ShortIdentityStrategy<Entity>.LastValue = seedNumber;
            target = new ShortIdentityStrategy<Entity>(x => x.Id);
        }

        [TestMethod]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            var result = target.Next();

            // Assert
            Assert.AreEqual(seedNumber + 1, result);
        }

        [TestMethod]
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