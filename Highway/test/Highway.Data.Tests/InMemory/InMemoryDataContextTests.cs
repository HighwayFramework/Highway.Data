using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Site = Highway.Data.Tests.InMemory.Domain.Site;

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class InMemoryDataContextTests
    {
        private InMemoryDataContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        [TestMethod]
        public void ShouldStoreASite()
        {
            //arrange 
            //act
            var item = new Site();
            _context.Add(item);

            var site = _context.AsQueryable<Site>().First();

            //assert
            site.Should().BeSameAs(item);
        }

        [TestMethod]
        public void ShouldStoreTwoSites()
        {
            //arrange 

            //act
            var item = new Site();
            _context.Add(item);
            var item2 = new Site();
            _context.Add(item2);

            //assert
            //TODO : Measured Equality instead of implicit ordering
            var site = _context.AsQueryable<Site>().First();
            site.Should().BeSameAs(item);
            var site2 = _context.AsQueryable<Site>().Last();
            site2.Should().BeSameAs(item2);
        }

        [TestMethod]
        public void ShouldStoreBlogWithAuthor()
        {
            //arrange 
            var blog = new Blog()
            {
                Author = new Author()
            };

            //act
            _context.Add(blog);

            //assert
            _context.AsQueryable<Author>().Count().Should().Be(1);
            _context.AsQueryable<Author>().First().Should().BeSameAs(blog.Author);
        }

        [TestMethod]
        public void ShouldStoreThreeLevelsOfObjects()
        {
            //arrange 
            var site = new Site()
            {
                Blog = new Blog()
                {
                    Author = new Author()
                }
            };


            //act
            _context.Add(site);

            //assert
            _context.AsQueryable<Author>().Count().Should().Be(1);
            _context.AsQueryable<Author>().First().Should().BeSameAs(site.Blog.Author);
        }

        [TestMethod]
        public void ShouldIncludeAllRelatedItemsFromRelatedCollections()
        {
            //arrange 
            var blog = new Blog()
            {
                Posts = new List<Post>() {new Post(), new Post()}
            };

            //act
            _context.Add(blog);

            //assert
            _context.AsQueryable<Post>().Count().Should().Be(2);
        }

        [TestMethod]
        public void ShouldIgnoreNullCollections()
        {
            //arrange 
            var blog = new Blog()
            {
                Posts = null
            };

            //act
            _context.Add(blog);

            //assert
            _context.AsQueryable<Post>().Count().Should().Be(0);

        }

        [TestMethod]
        public void ShouldIgnoreNonReferenceTypeProperties()
        {
            //arrange 
            var site = new Site();
            site.Id = 2;
            
            //act
            _context.Add(site);

            //assert
            _context.Data.Count(x => x.IsType<int>()).Should().Be(0);
        }

        [TestMethod]
        public void ShouldDeleteAnObjectFromRelatedObjects()
        {
            //arrange 
            var site = new Site() {Blog = new Blog()};

            //act
            _context.Add(site);
            _context.Remove(site.Blog);

            //assert
            _context.AsQueryable<Blog>().Count().Should().Be(0);
            _context.AsQueryable<Site>().First().Blog.Should().BeNull();

        }

        [TestMethod]
        public void ShouldRemoveObjectFromRelatedCollection()
        {
            //arrange 
            Post post = new Post();
            var blog = new Blog() { Posts = new List<Post>(){post} };

            //act
            _context.Add(blog);
            _context.Remove(post);

            //assert
            _context.AsQueryable<Blog>().Count().Should().Be(1);
            _context.AsQueryable<Blog>().First().Posts.Count().Should().Be(0);
            _context.AsQueryable<Post>().Count().Should().Be(0);

        }

        [TestMethod]
        public void ShouldRemoveDependentGraphOnBranchRemoval()
        {
            //arrange 
            var post = new Post();
            var blog = new Blog()
            {
                Posts = new List<Post>() { new Post(),post}
            };
            var site = new Site()
            {
                Blog = blog
            };
            _context.Add(site);

            //act
            _context.Remove(blog);

            //assert
            var posts = _context.AsQueryable<Post>();
            posts.Count().Should().Be(0);
        }
    }
}