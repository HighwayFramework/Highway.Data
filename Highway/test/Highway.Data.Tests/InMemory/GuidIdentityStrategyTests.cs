using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.Tests.InMemory.Domain;
using Highway.Data.Contexts;
using FluentAssertions;

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class GuidIdentityStrategyTests
    {
        private GuidIdentityStrategy<Blog> target;

        [TestInitialize]
        public void Setup()
        {
            target = new GuidIdentityStrategy<Blog>(x => x.Id);
        }

        [TestMethod]
        public void Next_ShouldReturnADifferentValueEachTime()
        {
            // Arrange
            Guid val1 = target.Next();

            // Act
            Guid val2 = target.Next();

            // Assert
            val1.Should().NotBe(val2);
        }


        [TestMethod]
        public void Assign_ShouldAssignIdOfBlog()
        {
            // Arrange
            var blog = new Blog { Id = Guid.Empty };

            // Act
            target.Assign(blog);

            // Assert
            blog.Id.Should().NotBe(Guid.Empty);
        }
    }
}
