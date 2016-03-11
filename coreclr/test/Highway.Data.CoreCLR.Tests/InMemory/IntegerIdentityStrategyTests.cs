using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class IntegerIdentityStrategyTests
    {
        private readonly int seedNumber = 500;
        private IntegerIdentityStrategy<Post> target;

        [SetUp]
        public void Setup()
        {
            IntegerIdentityStrategy<Post>.LastValue = seedNumber;
            target = new IntegerIdentityStrategy<Post>(x => x.Id);
        }

        [Test]
        public void Next_ShouldReturnNextValue()
        {
            // Arrange

            // Act
            int result = target.Next();

            // Assert
            result.Should().Be(seedNumber + 1);
        }

        [Test]
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
