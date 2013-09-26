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
    public class IntegerIdentityStrategyTests
    {
        private IntegerIdentityStrategy<Post> target;
        private readonly int seedNumber = 500;

        [TestInitialize]
        public void Setup()
        {
            IntegerIdentityStrategy<Post>.LastValue = seedNumber;
            target = new IntegerIdentityStrategy<Post>();
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
    }
}
