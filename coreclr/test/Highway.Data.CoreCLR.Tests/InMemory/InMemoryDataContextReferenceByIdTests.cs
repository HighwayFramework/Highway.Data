using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data.InMemory;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class InMemoryDataContextReferenceByIdTests
    {
        private InMemoryDataContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        [Test]
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
            public List<Post> Posts { get; set; }
        }
        class Post : IIdentifiable<long>
        {
            public long Id { get; set; }
            public long BlogId { get; set; }
        }
    }
}
