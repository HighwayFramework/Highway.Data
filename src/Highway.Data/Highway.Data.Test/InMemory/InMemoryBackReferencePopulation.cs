using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;


namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class InMemoryBackReferencePopulation
    {
        [TestMethod]
        public void ShouldPopulateSingleReference()
        {
            var context = new InMemoryDataContext();

            var child = new Post();
            var blog = new Blog("Test");
            blog.Posts.Add(child);

            context.Add(blog);
            context.Commit();

            Assert.IsNotNull(child.Blog);
        }

        [TestMethod]
        public void ShouldPopulateCollectionBasedReference()
        {
            var context = new InMemoryDataContext();

            var child = new Post();
            var blog = new Blog("Test");
            child.Blog = blog;

            context.Add(child);
            context.Commit();

            Assert.AreEqual(1, blog.Posts.Count(x => x == child));
        }

        [TestMethod]
        public void ShouldPopulateCollectionBasedReferenceReplacingNullCollection()
        {
            var context = new InMemoryDataContext();

            var child = new Post();
            var blog = new Blog("Test");
            child.Blog = blog;
            blog.Posts = null;

            context.Add(child);
            context.Commit();

            Assert.AreEqual(1, blog.Posts.Count(x => x == child));
        }
    }
}