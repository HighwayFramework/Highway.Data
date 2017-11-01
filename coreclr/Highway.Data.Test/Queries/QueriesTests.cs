using Highway.Data.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Highway.Data.Test.Queries
{
	[TestClass]
	public class QueriesTests : BaseQueriesTests
	{
		public class BlogPost : IIdentifiable<Guid>, ICoded
		{
			public Guid Id { get; set; }
			public string Code { get; set; }
			public string Name { get; set; }
		}

		private Guid sampleId = Guid.NewGuid();
		private Guid testId = Guid.NewGuid();
		private Guid exampleId = Guid.NewGuid();

		public override void ConfigureModelBuilder(ModelBuilder builder)
		{
			builder.Entity<BlogPost>(e => e.HasKey(x => x.Id));
		}
		public override void ArrangeDataContext(IDataContext context)
		{
			context.Add(new BlogPost { Id = sampleId, Code = "sample", Name = "Sample Posting" });
			context.Add(new BlogPost { Id = testId, Code = "test", Name = "Test Posting" });
			context.Add(new BlogPost { Id = exampleId, Code = "example", Name = "Example Posting" });
		}

		[TestMethod]
		public void FindAll_Should_Return_All_Entries()
		{
			// Act
			var result = repo.Find(new FindAll<BlogPost>());

			// Assert
			Assert.AreEqual(3, result.Count());
		}

		[TestMethod]
		public void GetById_Should_Return_Entity_When_Id_Correct()
		{
			// Act
			var result = repo.Find(new GetById<Guid, BlogPost>(sampleId));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BlogPost));
		}

		[TestMethod]
		public void GetById_Should_Return_Null_When_Id_Incorrect()
		{
			// Act
			var result = repo.Find(new GetById<Guid, BlogPost>(Guid.NewGuid()));

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetByCode_Should_Return_Entity_When_Code_Correct()
		{
			// Act
			var result = repo.Find(new GetByCode<BlogPost>("sample"));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BlogPost));
		}

		[TestMethod]
		public void GetByCode_Should_Return_Null_When_Code_Incorrect()
		{
			// Act
			var result = repo.Find(new GetByCode<BlogPost>("bad-data"));

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetIdByCode_Should_Return_Id_When_Code_Correct()
		{
			// Act
			var result = repo.Find(new GetIdByCode<Guid, BlogPost>("test"));

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(testId, result);
		}

		[TestMethod]
		public void GetIdByCode_Should_Return_Default_When_Code_Incorrect()
		{
			// Act
			var result = repo.Find(new GetIdByCode<Guid, BlogPost>("bad-data"));

			// Assert
			Assert.AreEqual(default(Guid), result);
		}
	}
}
