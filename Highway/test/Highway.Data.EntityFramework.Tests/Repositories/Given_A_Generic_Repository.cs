#region

using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.TestQueries;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

#endregion

namespace Highway.Data.EntityFramework.Tests.Repositories
{
    [TestClass]
    public class Given_A_Generic_Repository : ContainerTest<Repository>
    {
        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(
                Component.For<IDataContext>().ImplementedBy<TestDataContext>()
                    .DependsOn(new
                    {
                        connectionString = Settings.Default.Connection,
                        configurations = new[] {new FooMappingConfiguration()},
                    }),
                Component.For<IMappingConfiguration>()
                    .ImplementedBy<FooMappingConfiguration>(),
                Component.For<ILog>().ImplementedBy<NoOpLogger>());
            base.RegisterComponents(container);
        }

        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange
            var context = Container.Resolve<IDataContext>();

            //Act
            var repository = new Repository(context);

            //Assert
            repository.Context.ShouldBeSame(context);
        }

        [TestMethod]
        public void Should_Execute_Query_Objects()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                {
                    new Foo {Id = 1, Name = "Test"}
                }.AsQueryable());
            target = new Repository(context);

            //Act
            IEnumerable<Foo> result = target.Find(new FindFoo());

            //Assert
            context.VerifyAllExpectations();
            Foo foo = result.First();
            foo.ShouldNotBeNull();
            foo.Id.ShouldBe(1);
            foo.Name.ShouldBe("Test");
        }

        [TestMethod]
        public void Should_Execute_Scalar_Objects_That_Return_Values()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                {
                    new Foo {Id = 1, Name = "Test"}
                }.AsQueryable());
            target = new Repository(context);

            //Act
            int result = target.Find(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.ShouldBe(1);
        }

        [TestMethod]
        public void Should_Execute_Scalar_Objects_That_Return_Objects()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                {
                    new Foo {Id = 1, Name = "Test"}
                }.AsQueryable());
            target = new Repository(context);

            //Act
            int result = target.Find(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.ShouldBe(1);
        }

        [TestMethod]
        public void Should_Execute_A_Query_Object_And_Pull_The_First_Object()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            var foo = new Foo {Id = 1, Name = "Test"};
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                {
                    foo
                }.AsQueryable());
            target = new Repository(context);

            //Act
            Foo result = target.Find(new FindFoo()).FirstOrDefault();

            //Assert
            context.VerifyAllExpectations();
            result.ShouldBe(foo);
        }

        [TestMethod]
        public void Should_Execute_Commands_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                {
                    new Foo {Id = 1, Name = "Test"}
                }.AsQueryable());
            target = new Repository(context);

            //Act
            var testCommand = new TestCommand();
            target.Execute(testCommand);

            //Assert
            context.VerifyAllExpectations();
            testCommand.Called.ShouldBeTrue();
        }
    }
}