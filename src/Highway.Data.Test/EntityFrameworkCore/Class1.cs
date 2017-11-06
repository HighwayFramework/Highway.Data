using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Highway.Data.Test.EntityFrameworkCore
{
	public class CommandThatThrows : Command
	{
		public CommandThatThrows()
		{
			ContextQuery = c => throw new Exception("This is a test");
		}
	}

	[TestClass]
    public class DataContextTests
    {
		private IDataContext context;

		public TestContext TestContext { get; set; }

		[TestInitialize]
		public void Setup()
		{
			var mb = new ModelBuilder(new ConventionSet());
			mb.Entity<Person>(e => e.HasKey(x => x.Id));

			var builder = new DbContextOptionsBuilder()
				.UseInMemoryDatabase(TestContext.FullyQualifiedTestClassName)
				.UseModel(mb.Model);

			context = new DataContext(builder.Options);
		}

		[TestMethod]
		public void Commit_Should_Hand_Exceptions_To_AfterSave_Event()
		{
			// Arrange

			// Act
			context.Commit();

			// Assert

		}
	}
}
