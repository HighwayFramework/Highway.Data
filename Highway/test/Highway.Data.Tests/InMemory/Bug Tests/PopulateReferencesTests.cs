using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class PopulateReferencesTests
    {
        [TestMethod]
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

        [TestMethod]
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