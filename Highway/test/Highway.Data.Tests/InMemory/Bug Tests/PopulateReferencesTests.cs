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
            blog.AddPost(new Post());
            var post = blog.Posts.First();
            context.Add(post);
            context.Commit();

            // Act
            var fetchedBlog = context.AsQueryable<Blog>().First();

            // Assert
            fetchedBlog.Posts.Count().Should().Be(1);
            fetchedBlog.InvalidPosts.Should().BeNullOrEmpty();
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
            fetchedPost.Blog.InvalidPosts.Should().BeNullOrEmpty();
        }
    }
}