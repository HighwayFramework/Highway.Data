using System;
using System.Linq;
using System.Web;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.OData;
using Highway.Data.Tests.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.OData
{
    [TestClass]
    public class BasicODataQueryTests
    {
        [TestMethod]
        public void ShouldApplyPaging()
        {
            var uri = new Uri("http://localhost/Something/To/Test?$skip=1&$top=1");
            var context = GetTestContext();
            var resultingQueryable = context.AsQueryable<ExampleLeaf>().Filter(HttpUtility.ParseQueryString(uri.Query));

            var results = resultingQueryable.ToList();
            results.Should().HaveCount(1);
            results.Single().Id.Should().Be(2);
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
}