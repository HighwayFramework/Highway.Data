using Highway.Data.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Highway.Data.Tests.InMemory
{

    [TestClass]
    public class InMemoryDataContextReferenceByIdTests
    {
        private InMemoryDataContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        [TestMethod]
        public void ShouldBeAbleToIterate()
        {
            _context.Add(new Blog());
            _context.Add(new Blog());
            _context.Commit();

            try
            {
                foreach (var blogId in _context.AsQueryable<Blog>().Select(b => b.Id))
                {
                    var post = new Post { BlogId = blogId };
                    _context.Add(post);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        class Blog : IIdentifiable<long>
        {
            public Blog()
            {
                Posts = new List<Post>();
            }
            public long Id { get; set; }
            public List<Post> Posts { get; }
        }

        class Post : IIdentifiable<long>
        {
            public long Id { get; set; }
            public long BlogId { get; set; }
        }
    }
}