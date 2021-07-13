using Common.Logging.Simple;
using FluentAssertions;
using Highway.Data.Contexts;
using Highway.Data.EntityFramework.Test.TestDomain;
using Highway.Data.EntityFramework.Test.TestDomain.Queries;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Highway.Data.EntityFramework.Test
{

    [TestClass]
    public class Given_A_Generic_Repository
    {
        private IDataContext context;
        private IRepository target;

        [TestInitialize]
        public void Setup()
        {
            context = new TestDataContext(
                connectionString: "Data Source=(localDb);Initial Catalog=Highway.Data.Test.Db;Integrated Security=True",
                mapping: new FooMappingConfiguration(),
                logger: new NoOpLogger());
        }


        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange

            //Act
            var repository = new Repository(context);

            //Assert
            repository.Context.Should().BeSameAs(context);
        }

        [TestMethod]
        public void Should_Execute_Query_Objects()
        {
            //Arrange
            context = new InMemoryDataContext();
            context.Add(new Foo { Id = 1, Name = "Test" });
            context.Commit();
            target = new Repository(context);

            //Act
            IEnumerable<Foo> result = target.Find(new FindFoo());

            //Assert
            Foo foo = result.First();
            foo.Should().NotBeNull();
            foo.Id.Should().Be(1);
            foo.Name.Should().Be("Test");
        }

        [TestMethod]
        public void Should_Extend_Query_Objects()
        {
            //Arrange
            context = new InMemoryDataContext();
            context.Add(new Foo {Id = 1, Name = "Test"});
            context.Add(new Foo {Id = 2, Name = "Test2"});
            context.Add(new Foo {Id = 3, Name = "NoMatch"});
            context.Commit();
            target = new Repository(context);

            //Act
            var query = new FindFoo().Where(x => x.Name.Contains("Test")).Skip(1).Take(1);
            IEnumerable<Foo> result = target.Find(query);

            //Assert
            Foo foo = result.Single();
            foo.Should().NotBeNull();
            foo.Id.Should().Be(2);
            foo.Name.Should().Be("Test2");
        }

        [TestMethod]
        public void Should_Extend_Selector_Projector_Query_Objects()
        {
            //Arrange
            context = new InMemoryDataContext();

            // Create the first Foo, with two Bar children.  This is the non-matching Foo.
            var nonMatchingFoo = new Foo
            {
                Id = 1, Name = "Foo1", Bars = new List<Bar>()
                {
                    new Bar {Id = 1, Name = "Bar1"},
                    new Bar {Id = 2, Name = "Bar2"}
                }
            };

            // Create the second Foo, with three bar children.  This is the Foo we'll match in our query.
            // Only two of the bars will match the .Where extension on our query.
            var matchingFoo = new Foo
            {
                Id = 2, Name = "Foo2", Bars = new List<Bar>()
                {
                    new Bar {Id = 3, Name = "MatchingBar3"},
                    new Bar {Id = 4, Name = "MatchingBar4"},
                    new Bar {Id = 5, Name = "Bar5"}
                }
            };

            context.Add(nonMatchingFoo);
            context.Add(matchingFoo);
            context.Commit();
            target = new Repository(context);

            //Act
            var query = new FindBarByFooId(2).Where(x => x.Name.Contains("Matching")).Skip(1).Take(1);
            IEnumerable<Bar> result = target.Find(query);

            //Assert
            Bar bar = result.Single();
            bar.Should().NotBeNull();
            bar.Id.Should().Be(4);
            bar.Name.Should().Be("MatchingBar4");
        }

        [TestMethod]
        public void Should_Execute_Scalar_Objects_That_Return_Values()
        {
            //Arrange
            context = new InMemoryDataContext();
            context.Add(new Foo { Id = 1, Name = "Test" });
            context.Commit();
            target = new Repository(context);

            //Act
            int result = target.Find(new ScalarIntTestQuery());

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void Should_Execute_Scalar_Objects_That_Return_Objects()
        {
            //Arrange
            context = new InMemoryDataContext();
            context.Add(new Foo { Id = 1, Name = "Test" });
            context.Commit();
            target = new Repository(context);

            //Act
            int result = target.Find(new ScalarIntTestQuery());

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void Should_Execute_A_Query_Object_And_Pull_The_First_Object()
        {
            //Arrange
            context = new InMemoryDataContext();
            Foo foo = new Foo { Id = 1, Name = "Test" };
            context.Add(foo);
            context.Commit();
            target = new Repository(context);

            //Act
            Foo result = target.Find(new FindFoo()).FirstOrDefault();

            //Assert
            result.Should().Be(foo);
        }

        [TestMethod]
        public void Should_Execute_Commands_Against_Context()
        {
            //Arrange
            context = new InMemoryDataContext();
            Foo foo = new Foo { Id = 1, Name = "Test" };
            context.Add(foo);
            target = new Repository(context);

            //Act
            var testCommand = new TestCommand();
            target.Execute(testCommand);

            //Assert
            testCommand.Called.Should().BeTrue();
        }
    }
}
