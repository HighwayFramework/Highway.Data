using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Highway.Data.Tests.InMemory
{

    [TestClass]
    public class InMemoryDataContextPerfTests
    {
        private InMemoryDataContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        [TestMethod]
        public void ShouldPerformBetterThan10MsInserts()
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
                        Posts = new List<Post> { new Post(), new Post() }
                    }
                });
            }
            sw.Stop();
            var averageInsert = sw.ElapsedMilliseconds / 1000;
            averageInsert.Should().BeLessOrEqualTo(10);
            Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
        }

        [TestMethod]
        public void ShouldPerformCommitsBetterThan10Ms()
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
            var averageInsert = sw.ElapsedMilliseconds / 1000;
            averageInsert.Should().BeLessOrEqualTo(10);
            Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
        }
    }
}