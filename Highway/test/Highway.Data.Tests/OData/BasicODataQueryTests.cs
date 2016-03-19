using System;
using System.Linq;
using System.Web;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.OData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.OData
{
    [TestClass]
    public class BasicODataQueryTests
    {
        [TestMethod]
        public void ShouldApplyPaging()
        {
            var uri = new Uri("http://localhost/Something/To/Test?$inlinecount=allpages&$skip=1&$top=1");
            var context = GetTestContext();
            var repo = new Repository(context);

            var odataResponse = repo.Find(new GetByOData<ExampleLeaf>(HttpUtility.ParseQueryString(uri.Query)));
            odataResponse.Count.Should().Be(3);
            odataResponse.Results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldApplyOrderingDesc()
        {
            var uri = new Uri("http://localhost/Something/To/Test?$orderby=Id desc");
            var context = GetTestContext();
            var resultingQueryable = context.AsQueryable<ExampleLeaf>().Filter(HttpUtility.ParseQueryString(uri.Query));

            var results = resultingQueryable.ToList();
            results.First().Id.Should().Be(3);
            results.Last().Id.Should().Be(1);
        }

        [TestMethod]
        public void ShouldApplyOrderingDescOnMultipleFields()
        {
            var uri = new Uri("http://localhost/Something/To/Test?$orderby=Name, Id");
            var context = GetTestContext();
            var resultingQueryable = context.AsQueryable<ExampleRoot>().Filter(HttpUtility.ParseQueryString(uri.Query));

            var results = resultingQueryable.ToList();
            results.First().Id.Should().Be(4);
            results.Last().Id.Should().Be(2);
        }

        private static InMemoryDataContext GetTestContext()
        {
            var inMemoryDataContext = new InMemoryDataContext();
            inMemoryDataContext.Add(new ExampleLeaf()
            {
                Id = 1,
                SecuredRoot = new ExampleRoot()
                {
                    Id = 1,
                    Name = "Example Root"
                }
            });
            inMemoryDataContext.Add(new ExampleLeaf()
            {
                Id = 2,
                SecuredRoot = new ExampleRoot()
                {
                    Id = 2,
                    Name = "Example Root"
                }
            });
            inMemoryDataContext.Add(new ExampleLeaf()
            {
                Id = 3,
                SecuredRoot = new ExampleRoot()
                {
                    Id = 4,
                    Name = "A First Record"
                }
            });
            inMemoryDataContext.Commit();
            return inMemoryDataContext;
        }
    }

    public class ExampleRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ExampleLeaf
    {
        public int Id { get; set; }
        public ExampleRoot SecuredRoot { get; set; }
    }
}
