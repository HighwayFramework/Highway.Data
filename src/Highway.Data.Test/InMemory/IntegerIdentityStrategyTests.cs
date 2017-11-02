using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.Test.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Highway.Data.Test.InMemory
{
	[TestClass]
	public class IntegerIdentityStrategyTests
	{
		private readonly int seedNumber = 500;
		private IntegerIdentityStrategy<Post> target;

		[TestInitialize]
		public void Setup()
		{
			target = new IntegerIdentityStrategy<Post>(x => x.Id);
			target.LastValue = seedNumber;
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
			var post = new Post { Id = 0 };

			// Act
			target.Assign(post);

			// Assert
			post.Id.Should().Be(seedNumber + 1);
		}
	}
}