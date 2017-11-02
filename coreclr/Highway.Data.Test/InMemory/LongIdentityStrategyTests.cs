using FluentAssertions;
using Highway.Data.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Highway.Data.Test.InMemory
{
	[TestClass]
	public class LongIdentityStrategyTests
	{
		private readonly long seedNumber = 500;
		private LongIdentityStrategy<Entity> target;

		[TestInitialize]
		public void Setup()
		{
			target = new LongIdentityStrategy<Entity>(x => x.Id);
			target.LastValue = seedNumber;
		}

		[TestMethod]
		public void Next_ShouldReturnNextValue()
		{
			// Arrange

			// Act
			var result = target.Next();

			// Assert
			result.Should().Be(seedNumber + 1);
		}

		[TestMethod]
		public void Assign_ShouldAssignId()
		{
			// Arrange
			var entity = new Entity { Id = 0 };

			// Act
			target.Assign(entity);

			// Assert
			entity.Id.Should().Be(seedNumber + 1);
		}

		class Entity
		{
			public long Id { get; set; }
		}
	}
}