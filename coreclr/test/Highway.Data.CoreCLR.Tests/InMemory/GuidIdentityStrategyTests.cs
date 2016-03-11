using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;
using System;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class GuidIdentityStrategyTests
    {
        private GuidIdentityStrategy<Blog> target;

        [SetUp]
        public void Setup()
        {
            target = new GuidIdentityStrategy<Blog>(x => x.Id);
        }

        [Test]
        public void Next_ShouldReturnADifferentValueEachTime()
        {
            // Arrange
            Guid val1 = target.Next();

            // Act
            Guid val2 = target.Next();

            // Assert
            val1.Should().NotBe(val2);
        }


        [Test]
        public void Assign_ShouldAssignIdOfBlog()
        {
            // Arrange
            var blog = new Blog {Id = Guid.Empty};

            // Act
            target.Assign(blog);

            // Assert
            blog.Id.Should().NotBe(Guid.Empty);
        }
    }
}
