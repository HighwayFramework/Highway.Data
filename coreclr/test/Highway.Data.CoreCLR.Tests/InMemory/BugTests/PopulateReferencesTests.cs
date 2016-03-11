using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory.BugTests
{
    [TestFixture]
    public class PopulateReferencesTests
    {
        [Test]
        public void ShouldPopulateBackReferenceCollections()
        {
            // Arrange
            var context = new InMemoryDataContext();
            var blog = new Blog();
            var post = new Post { Blog = blog };
            context.Add(post);
            context.Commit();

            // Act
            var fetchedBlog = context.AsQueryable<Blog>().First();

            // Assert
            fetchedBlog.Posts.Count().Should().Be(1);
        }

        [Test]
        public void ShouldPopulateBackReferenceSingleProperty()
        {
            // Arrange
            var context = new InMemoryDataContext();
            var blog = new Blog()
            {
                Posts = new List<Post>()
                {
                    new Post()
                }
            };
            context.Add(blog);
            context.Commit();

            // Act
            var fetchedPost = context.AsQueryable<Post>().First();

            // Assert
            fetchedPost.Blog.Should().NotBeNull();
        }
    }
}
