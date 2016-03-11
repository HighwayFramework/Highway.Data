using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class InMemoryBackReferencePopulation
    {
        [Test]
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

        [Test]
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

        [Test]
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
