using System.Linq;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Highway.Data.Tests.InMemory.ScenarioTests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.ScenarioTests
{
    [TestClass]
    public class BlogLifeTime
    {
        private IRepository repo;

        [TestInitialize]
        public void Setup()
        {
            repo = new Repository(new InMemoryDataContext());
        }

        [TestMethod]
        public void ShouldCreateStoreAndRetrieve()
        {
            //Arrange

            var author = new Author()
            {
                FirstName = "Devlin",
                LastName = "Liles",
                Email = "devlin@devlinliles.com",
                TwitterHandle = "@DevlinLiles"
            };
           
            
            //Act - Scenario
            var blogService = new TestBlogService(repo);
            blogService.StartBlog("Testing",author);

            blogService.Post("Testing", new Post()
            {
                Title = "Test One",
                Body = "This is a body paragraph"

            });

            //Assert
            blogService.Posts("Testing").Count().Should().Be(1);
            (repo.Context as IDataContext).AsQueryable<Post>().Count().Should().Be(1);
        }
    }
}