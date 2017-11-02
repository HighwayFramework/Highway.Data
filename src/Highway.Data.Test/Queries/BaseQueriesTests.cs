using Highway.Data.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Highway.Data.Test.Queries
{
	[TestClass]
    public abstract class BaseQueriesTests
    {
		protected IRepository repo;

		public TestContext TestContext { get; set; }

		public abstract void ArrangeDataContext(IDataContext context);
		public abstract void ConfigureModelBuilder(ModelBuilder builder);

		[TestInitialize]
		public void Setup()
		{
			var mb = new ModelBuilder(new ConventionSet());
			ConfigureModelBuilder(mb);

			var builder = new DbContextOptionsBuilder()
				.UseInMemoryDatabase(TestContext.FullyQualifiedTestClassName)
				.UseModel(mb.Model);

			IDataContext dc = new DataContext(builder.Options);

			ArrangeDataContext(dc);
			dc.Commit();

			repo = new Repository(dc);
		}
    }
}
