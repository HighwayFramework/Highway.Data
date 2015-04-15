
using System.Linq;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Highway.Data.Tests.InMemory.ScenarioTests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.PrebuiltQueries;
using System;


namespace Highway.Data.Tests.InMemory.ScenarioTests
{
    [TestClass]
    public class FindWhereTests
    {
        private IRepository repo;

        [TestInitialize]
        public void Setup()
        {
            repo = new Repository(new InMemoryDataContext());

            var author = new Author
            {
                FirstName = "Devlin",
                LastName = "Liles",
                Email = "devlin@devlinliles.com",
                TwitterHandle = "@DevlinLiles"
            };


            //Act - Scenario
            var blogService = new TestBlogService(repo);
            blogService.StartBlog("Testing", author);

            blogService.Post("Testing", new Post
            {
                Title = "Test One",
                Body = "This is a body paragraph"
            });

            blogService.Post("Testing", new Post
            {
                Title = "Test Two",
                Body = "This is a body paragraph"
            });

            blogService.Post("Testing", new Post
            {
                Title = "Test first",
                Body = "This is a first"
            });

            blogService.Post("Testing", new Post
            {
                Title = "Test First",
                Body = "This is a real first (for testing ascii order F<f)"
            });
        }

        [TestMethod]
        public void ShouldFindByWhereCondition()
        {
            var posts = repo.Find(new FindWhere<Post>(p => p.Title == "Test Two"));
            posts.Count().Should().Be(1);
        }

        [TestMethod]
        public void ShouldOrderTheResults()
        {
            var posts = repo.Find(
                new FindWhere<Post>(p => p.Title.StartsWith("Test") && !p.Title.Contains("Two"))
                .OrderedBy(p => p.Title));
            posts.Count().Should().Be(3);
            // first, lowercase, has been inserted before First, uppercase.
            posts.First().Title.Should().Be("Test first");
            posts.Last().Title.Should().Be("Test One");
        }

        [TestMethod]
        public void ShouldOrderTheResultsWithAComparer()
        {
            var posts = repo.Find(
                new FindWhere<Post>(p => p.Title.Contains("irst"))
                .OrderedBy(p => p.Title, StringComparer.OrdinalIgnoreCase));
            posts.Count().Should().Be(2);
            posts.First().Title.Should().Be("Test first");
            posts.Last().Title.Should().Be("Test First");
        }
    }
}