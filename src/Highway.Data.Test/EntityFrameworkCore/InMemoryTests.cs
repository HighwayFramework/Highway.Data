using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq;

namespace Highway.Data.Test
{
	public class Person
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class FirstRayburn : Scalar<Person>
	{
		public FirstRayburn()
		{
			ContextQuery = c => c.AsQueryable<Person>()
				.First(e => e.LastName == "Rayburn");
		}
	}
	[TestClass]
    public class InMemoryTests
    {
        [TestMethod]
        public void ShouldStoreAndRetrive()
        {
			var mb = new ModelBuilder(new ConventionSet());
			mb.Entity<Person>(e => e.HasKey(x => x.Id));

			var builder = new DbContextOptionsBuilder()
				.UseInMemoryDatabase("ShouldStoreAndRetrive")
				.UseModel(mb.Model);

			IDataContext dc = new DataContext(builder.Options);
			IRepository repo = new Repository(dc);

			repo.Context.Add(new Person { FirstName = "Tim", LastName = "Rayburn" });
			repo.Context.Commit();

			Assert.AreEqual("Tim", repo.Find(new FirstRayburn()).FirstName);
        }
    }
}
