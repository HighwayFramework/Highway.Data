using Highway.Data;
using Highway.RestArea.Test.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using System;
using System.Linq;

namespace Highway.RestArea.Test
{
	[TestClass]
	public class RestAreaOptionsBuilderTests : UnitOfWorkTest
	{
		private RestAreaOptionsBuilder<UnitOfWork> target;

		[TestInitialize]
		public void Setup()
		{
			target = new RestAreaOptionsBuilder<UnitOfWork>(uow.Model);
		}

		[TestMethod]
		public void Should_Build_Empty()
		{
			// Arrange
			// Act
			var result = target.Build();

			// Assert
			result.Entities.Should().BeEmpty();
		}


		[TestMethod]
		public void AddEntityById_Should_AddOneEntity()
		{
			// Arrange
			target.AddEntityById<Blog, Guid, string>(e => { });
			// Act
			var result = target.Build();

			// Assert
			result.Entities.Should().HaveCount(1);
			EntityOptions entity = result.Entities.First();
			entity.EntityType.Should().Be(typeof(Blog));
			entity.IdentityType.Should().Be(typeof(Guid));
			entity.ModelType.Should().Be(typeof(string));
		}

		[TestMethod]
		public void AddTransform_Should_ShouldConfigureGetMapper()
		{
			// Arrange
			target.AddTransform(cfg =>
			{
				cfg.CreateMap<string, Guid>().ProjectUsing(s => new Guid(s));
			});

			// Act
			var result = target.Build();

			// Assert
			const string guidString = "B6357421-BDCD-4B6A-B06F-8EEF9E53C83C";
			var guid = result.GetMapper().Map<Guid>(guidString);
			guid.Should().Be(new Guid(guidString));
		}

	}
}
