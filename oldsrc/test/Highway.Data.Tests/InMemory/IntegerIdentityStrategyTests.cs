using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class IntegerIdentityStrategyTests
    {
        private IntegerIdentityStrategy<Post> target;

        [TestInitialize]
        public void Setup()
        {
            target = new IntegerIdentityStrategy<Post>(x => x.Id);
        }

        [TestMethod]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            int result = target.Next();

            // Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void Assign_ShouldAssignIdOfPost()
        {
            // Arrange
            var post = new Post {Id = 0};

            // Act
            target.Assign(post);

            // Assert
            post.Id.Should().Be(1);
        }
    }
}