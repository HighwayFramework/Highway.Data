using FluentAssertions;

using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class IntegerIdentityStrategyTests
    {
        private IntegerIdentityStrategy<Post> _target;

        [TestMethod]
        public void Assign_ShouldAssignIdOfPost()
        {
            // Arrange
            var post = new Post { Id = 0 };

            // Act
            _target.Assign(post);

            // Assert
            post.Id.Should().Be(1);
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
            _target = new IntegerIdentityStrategy<Post>(x => x.Id);
        }
    }
}
