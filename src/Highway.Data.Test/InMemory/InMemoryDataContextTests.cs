using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.Tests.InMemory.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;


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
        public void Add_ShouldCreateAFirstIdOfOneEveryTimeForANewInstanceOfIIdentifiableOfInt()
        {
            // Arrange
            var longPerson = new IdentifiablePerson<int>();
            _context.Add(longPerson);
            _context.Commit();

            // Act
            var newLongPerson = new IdentifiablePerson<int>();
            _context = new InMemoryDataContext();
            _context.Add(newLongPerson);
            _context.Commit();

            // Assert
            newLongPerson.Id.Should().Be(1);
        }

        [TestMethod]
        public void Add_ShouldCreateAFirstIdOfOneEveryTimeForANewInstanceOfIIdentifiableOfLong()
        {
            // Arrange
            var longPerson = new IdentifiablePerson<long>();
            _context.Add(longPerson);
            _context.Commit();

            // Act
            var newLongPerson = new IdentifiablePerson<long>();
            _context = new InMemoryDataContext();
            _context.Add(newLongPerson);
            _context.Commit();

            // Assert
            newLongPerson.Id.Should().Be(1);
        }

        [TestMethod]
        public void Add_ShouldCreateAFirstIdOfOneEveryTimeForANewInstanceOfIIdentifiableOfShort()
        {
            // Arrange
            var longPerson = new IdentifiablePerson<short>();
            _context.Add(longPerson);
            _context.Commit();

            // Act
            var newLongPerson = new IdentifiablePerson<short>();
            _context = new InMemoryDataContext();
            _context.Add(newLongPerson);
            _context.Commit();

            // Assert
            newLongPerson.Id.Should().Be(1);
        }

        [TestMethod]
        public void Add_ShouldStoreASite()
        {
            //arrange 
            var item = new Site();

            //act
            _context.Add(item);
            _context.Commit();

            //assert
            Site site = _context.AsQueryable<Site>().First();
            site.Should().BeSameAs(item);
        }

        [TestMethod]
        public void Add_ShouldStoreTwoSites()
        {
            //arrange 
            var item = new Site();
            var item2 = new Site();

            //act
            _context.Add(item);
            _context.Add(item2);
            _context.Commit();

            //assert
            _context.AsQueryable<Site>()
                .Any(s => ReferenceEquals(item, s))
                .Should().BeTrue();
            _context.AsQueryable<Site>()
                .Any(s => ReferenceEquals(item2, s))
                .Should().BeTrue();
        }

        [TestMethod]
        public void Add_ShouldStoreBlogWithAuthor()
        {
            //arrange 
            var blog = new Blog
            {
                Author = new Author()
            };

            //act
            _context.Add(blog);
            _context.Commit();

            //assert
            _context.AsQueryable<Author>().Count().Should().Be(1);
            _context.AsQueryable<Author>().First().Should().BeSameAs(blog.Author);
        }

        [TestMethod]
        public void Add_ShouldStoreThreeLevelsOfObjects()
        {
            //arrange 
            var site = new Site
            {
                Blog = new Blog
                {
                    Author = new Author()
                }
            };


            //act
            _context.Add(site);
            _context.Commit();

            //assert
            _context.AsQueryable<Author>().Count().Should().Be(1);
            _context.AsQueryable<Author>().First().Should().BeSameAs(site.Blog.Author);
        }

        [TestMethod]
        public void Add_ShouldIncludeAllRelatedItemsFromRelatedCollections()
        {
            //arrange 
            var blog = new Blog
            {
                Posts = new List<Post> { new Post(), new Post() }
            };

            //act
            _context.Add(blog);
            _context.Commit();

            //assert
            _context.AsQueryable<Post>().Count().Should().Be(2);
        }

        [TestMethod]
        public void Add_ShouldNotAddAnItemTwiceForMultipleReferences()
        {
            //arrange 
            var post = new Post();
            var blog = new Blog
            {
                Posts = new List<Post> { post, new Post() }
            };
            var blog2 = new Blog
            {
                Posts = new List<Post> { post }
            };
            _context.Add(blog);
            _context.Commit();

            //act
            _context.Add(blog2);
            _context.Commit();

            //assert
            _context.AsQueryable<Post>().Count().Should().Be(2);
        }

        [TestMethod]
        public void Add_ShouldIgnoreNullCollections()
        {
            //arrange 
            var blog = new Blog
            {
                Posts = null
            };

            //act
            _context.Add(blog);

            //assert
            _context.AsQueryable<Post>().Count().Should().Be(0);
        }

        [TestMethod]
        public void Add_ShouldIgnoreNonReferenceTypeProperties()
        {
            //arrange 
            var site = new Site();
            site.Id = 2;

            //act
            _context.Add(site);
            _context.Commit();

            //assert
            _context.Repo.ObjectRepresentations.Count(x => x.IsType<int>()).Should().Be(0);
        }

        [TestMethod]
        public void Remove_ShouldDeleteAnObjectFromRelatedObjects()
        {
            //arrange 
            var site = new Site { Blog = new Blog() };
            _context.Add(site);
            _context.Commit();

            //act
            _context.Remove(site.Blog);
            _context.Commit();


            //assert
            _context.AsQueryable<Blog>().Count().Should().Be(0);
            _context.AsQueryable<Site>().First().Blog.Should().BeNull();
        }

        [TestMethod]
        public void Remove_ShouldRemoveObjectFromRelatedCollection()
        {
            //arrange 
            var post = new Post();
            var blog = new Blog { Posts = new List<Post> { post } };
            _context.Add(blog);
            _context.Commit();

            //act
            _context.Remove(post);
            _context.Commit();

            //assert
            _context.AsQueryable<Blog>().Count().Should().Be(1);
            _context.AsQueryable<Blog>().First().Posts.Count().Should().Be(0);
            _context.AsQueryable<Post>().Count().Should().Be(0);
        }

        [TestMethod]
        public void Remove_ShouldRemoveDependentGraphOnBranchRemoval()
        {
            //arrange 
            var post = new Post();
            var blog = new Blog
            {
                Posts = new List<Post> { new Post(), post }
            };
            var site = new Site
            {
                Blog = blog
            };
            _context.Add(site);
            _context.Commit();

            //act
            _context.Remove(blog);
            _context.Commit();

            //assert
            IQueryable<Post> posts = _context.AsQueryable<Post>();
            posts.Count().Should().Be(0);
        }

        [TestMethod]
        public void Remove_ShouldNotRemoveIfReferencedByAnotherObject()
        {
            // Arrange
            var blog = new Blog();
            var site1 = new Site { Blog = blog };
            var site2 = new Site { Blog = blog };
            _context.Add(site1);
            _context.Add(site2);
            _context.Commit();

            // Act
            _context.Remove(site1);
            _context.Commit();

            // Assert
            _context.AsQueryable<Site>().Count().Should().Be(1);
            _context.AsQueryable<Site>().First().Should().BeSameAs(site2);
            _context.AsQueryable<Blog>().Count().Should().Be(1);
            _context.AsQueryable<Blog>().First().Should().BeSameAs(blog);
        }

        [TestMethod]
        public void Remove_ShouldNotRemoveIfReferencedByAnotherCollection()
        {
            // Arrange
            var post1 = new Post();
            var post2 = new Post();
            var blog1 = new Blog
            {
                Posts = new List<Post> { post1, post2 }
            };
            var blog2 = new Blog
            {
                Posts = new List<Post> { post1 }
            };
            _context.Add(blog1);
            _context.Add(blog2);
            _context.Commit();

            // Act
            _context.Remove(post2);
            _context.Commit();

            // Assert
            _context.AsQueryable<Post>().Count().Should().Be(1);
            _context.AsQueryable<Post>().First().Should().BeSameAs(post1);
            _context.AsQueryable<Blog>().Where(b => b.Posts.Count > 1).Count().Should().Be(0);
        }

        [TestMethod]
        public void Remove_ShouldRemoveFromParentButNotDeleteChildObjectsThatAreReferencedMoreThanOne()
        {
            //arrange 

            var post1 = new Post();
            var post2 = new Post();
            var blog1 = new Blog
            {
                Posts = new List<Post> { post1, post2 }
            };
            var blog2 = new Blog
            {
                Posts = new List<Post> { post1 }
            };
            var site = new Site
            {
                Blog = blog2
            };
            _context.Add(blog1);
            _context.Add(site);
            _context.Commit();

            // Act
            _context.Remove(blog2);
            _context.Commit();

            // Assert
            _context.AsQueryable<Post>().Count().Should().Be(2);
            _context.AsQueryable<Post>().First().Should().BeSameAs(post1);
            _context.AsQueryable<Blog>().Single().Posts.Count().Should().Be(2);
            site.Blog.Should().BeNull();
        }

        [TestMethod]
        public void Commit_ShouldRemoveOrphanedCollectionMembers()
        {
            // Arrange
            var post1 = new Post();
            var post2 = new Post();
            var blog = new Blog
            {
                Posts = new List<Post> { post1, post2 }
            };
            _context.Add(blog);
            _context.Commit();

            blog.Posts.Remove(post2);

            // Act
            _context.Commit();

            // Assert
            _context.AsQueryable<Post>().Should().Contain(post1);
            _context.AsQueryable<Post>().Should().NotContain(post2);
        }

        [TestMethod]
        public void Commit_ShouldRemoveOrphanedCollectionMembersWhenWholeCollectionRemoved()
        {
            // Arrange
            var post1 = new Post();
            var post2 = new Post();
            var blog = new Blog
            {
                Posts = new List<Post> { post1, post2 }
            };
            _context.Add(blog);
            _context.Commit();
            blog.Posts = null;

            // Act
            _context.Commit();

            // Assert
            _context.AsQueryable<Post>().Should().NotContain(post1);
            _context.AsQueryable<Post>().Should().NotContain(post2);
            _context.AsQueryable<Post>().Count().Should().Be(0);
        }

        [TestMethod]
        public void Commit_ShouldRemoveOrphanedMembers()
        {
            // Arrange
            var blog = new Blog();
            var site = new Site { Blog = blog };
            _context.Add(site);
            _context.Commit();
            site.Blog = null;

            // Act
            _context.Commit();

            // Assert
            _context.AsQueryable<Blog>().Should().NotContain(blog);
            _context.AsQueryable<Blog>().Count().Should().Be(0);
        }

        [TestMethod]
        public void Commit_ShouldAddNewLeafMembers()
        {
            //Arrange
            var blog = new Blog();
            _context.Add(blog);
            _context.Commit();

            //Act
            blog.Posts.Add(new Post());
            _context.Commit();

            //Assert
            _context.AsQueryable<Post>().Count().Should().Be(1);
        }

        [TestMethod]
        public void Add_ShouldUseIdentityForType()
        {
            //Arrange
            _context.RegisterIdentityStrategy(new IntegerIdentityStrategy<Post>(x => x.Id));
            var post = new Post { Id = 0 };

            //Act
            _context.Add(post);
            _context.Commit();

            //Assert
            post.Id.Should().NotBe(0);
        }

        [TestMethod]
        public void Add_ShouldUseIdentityForRelatedCollectionTypes()
        {
            //Arrange
            _context.RegisterIdentityStrategy(new IntegerIdentityStrategy<Post>(x => x.Id));
            var blog = new Blog();
            blog.Posts.Add(new Post { Id = 0 });

            //Act
            _context.Add(blog);
            _context.Commit();

            //Assert
            blog.Posts.Single().Id.Should().NotBe(0);
        }

        [TestMethod]
        public void Add_ShouldUseIdentityForRelatedTypes()
        {
            //Arrange
            _context.RegisterIdentityStrategy(new GuidIdentityStrategy<Author>(x => x.Id));
            var blog = new Blog();
            blog.Author = new Author { Id = Guid.Empty };

            //Act
            _context.Add(blog);
            _context.Commit();

            //Assert
            blog.Author.Id.Should().NotBe(Guid.Empty);
        }


        [TestMethod]
        public void Commit_ShouldUseIdentityForRelatedCollectionTypes()
        {
            //Arrange
            _context.RegisterIdentityStrategy(new IntegerIdentityStrategy<Post>(x => x.Id));
            var blog = new Blog();
            _context.Add(blog);
            _context.Commit();
            blog.Posts.Add(new Post { Id = 0 });

            //Act
            _context.Commit();

            //Assert
            blog.Posts.Single().Id.Should().NotBe(0);
        }
    }
}