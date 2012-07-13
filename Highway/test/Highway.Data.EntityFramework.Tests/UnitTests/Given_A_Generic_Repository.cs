using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Repositories;
using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.Tests.TestDomain;
using Highway.Data.Tests.TestQueries;
using Highway.Test.MSTest;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Generic_Repository
    {
        private static IWindsorContainer container;

        [ClassInitialize]
        public static void SetupClass(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDataContext>().ImplementedBy<TestContext>().DependsOn(new { connectionString = Settings.Default.Connection, configurations = new []{new TestMappingConfiguration()} }).LifestyleTransient(),
                               Component.For<IMappingConfiguration>().ImplementedBy<TestMappingConfiguration>().LifestyleTransient());

        }
        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange
            var context = container.Resolve<IDataContext>();

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
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new Repository(context);
            
            //Act
            var result = target.Find(new FindFoo());

            //Assert
            context.VerifyAllExpectations();
            var foo = result.First();
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
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new Repository(context);

            //Act
            var result = target.Get(new ScalarIntTestQuery());

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
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new Repository(context);

            //Act
            var result = target.Get(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.ShouldBe(1);
        }

        [TestMethod]
        public void Should_Execute_A_Query_Object_And_Pull_The_First_Object()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            var foo = new Foo() {Id = 1, Name = "Test"};
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                    {
                        foo
                    }.AsQueryable());
            var target = new Repository(context);

            //Act
            var result = target.Find(new FindFoo()).FirstOrDefault();

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
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new Repository(context);

            //Act
            var testCommand = new TestCommand();
            target.Execute(testCommand);

            //Assert
            context.VerifyAllExpectations();
            testCommand.Called.ShouldBeTrue();
        }
    }
}
