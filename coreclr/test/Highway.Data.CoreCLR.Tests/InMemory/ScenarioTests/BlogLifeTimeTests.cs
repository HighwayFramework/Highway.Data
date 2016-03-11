using System.Linq;
using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using Highway.Data.CoreCLR.Tests.InMemory.ScenarioTests.Services;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory.ScenarioTests
{
    [TestFixture]
    public class BlogLifeTimeTests
    {
        private IRepository repo;

        [SetUp]
        public void Setup()
        {
            repo = new Repository(new InMemoryDataContext());
        }

        [Test]
        public void ShouldCreateStoreAndRetrieve()
        {
            //Arrange

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

            //Assert
            blogService.Posts("Testing").Count().Should().Be(1);
            (repo.Context as IDataContext).AsQueryable<Post>().Count().Should().Be(1);
        }
    }
}
