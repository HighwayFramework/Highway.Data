#region

using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class IntegerIdentityStrategyTests
    {
        private readonly int seedNumber = 500;
        private IntegerIdentityStrategy<Post> target;

        [TestInitialize]
        public void Setup()
        {
            IntegerIdentityStrategy<Post>.LastValue = seedNumber;
            target = new IntegerIdentityStrategy<Post>(x => x.Id);
        }

        [TestMethod]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            int result = target.Next();

            // Assert
            result.Should().Be(seedNumber + 1);
        }

        [TestMethod]
        public void Assign_ShouldAssignIdOfPost()
        {
            // Arrange
            var post = new Post {Id = 0};

            // Act
            target.Assign(post);

            // Assert
            post.Id.Should().Be(seedNumber + 1);
        }
    }
}