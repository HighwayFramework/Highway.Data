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
        private GuidIdentityStrategy<Post> target;
        private readonly Guid seedValue = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            target = new GuidIdentityStrategy<Post>();
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
    }
}
