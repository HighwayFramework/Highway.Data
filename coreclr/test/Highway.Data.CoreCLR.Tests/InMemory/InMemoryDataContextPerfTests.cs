using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class InMemoryDataContextPerfTests
    {
        private InMemoryDataContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        [Test]
        public void ShouldPerformBetterThan10msInserts()
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                _context.Add(new Site
                {
                    Blog = new Blog
                    {
                        Author = new Author(),
                        Id = Guid.NewGuid(),
                        Posts = new List<Post> {new Post(), new Post()}
                    }
                });
            }
            sw.Stop();
            var averageInsert = sw.ElapsedMilliseconds/1000;
            averageInsert.Should().BeLessOrEqualTo(10);
            Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
        }

        [Test]
        public void ShouldPerformCommitsBetterThan10ms()
        {
            //Arrange
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var blog = new Blog();
                _context.Add(blog);
                blog.Posts.Add(new Post());
                _context.Commit();
            }
            sw.Stop();
            var averageInsert = sw.ElapsedMilliseconds/1000;
            averageInsert.Should().BeLessOrEqualTo(10);
            Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
        }
    }
}
