using Highway.Data;
using Highway.RestArea.Test.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Highway.RestArea.Test
{
	[TestClass]
	public abstract class UnitOfWorkTest
	{
		protected UnitOfWork uow;
		protected IRepository repo;

		public TestContext TestContext { get; set; }

		[TestInitialize]
		public void UnitOfWorkSetup()
		{
			var csBuilder = new CoreConventionSetBuilder(
				new CoreConventionSetBuilderDependencies(
					new CoreTypeMapper(
						new CoreTypeMapperDependencies())));

			var mb = new ModelBuilder(csBuilder.CreateConventionSet());
			mb.Entity<Blog>();
			mb.Entity<Post>();
			mb.Entity<Category>();

			var builder = new DbContextOptionsBuilder()
				.UseInMemoryDatabase(TestContext.TestName)
				.UseModel(mb.Model);

			uow = new UnitOfWork(builder.Options);
			repo = new Repository(uow);

			repo.UnitOfWork.Add(new Blog
			{
				Title = "Test Title",
				Posts = new List<Post>()
				{
					new Post
					{
						Title = "Foo",
						Body = "Bar",
						Categories = new List<Category>
						{
							new Category { Name = "Awesome" }
						}
					}
				}
			});
			repo.UnitOfWork.Commit();
		}
	}
}
