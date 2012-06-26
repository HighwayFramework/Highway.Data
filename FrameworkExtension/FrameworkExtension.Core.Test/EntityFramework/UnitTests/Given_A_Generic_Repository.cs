using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using FrameworkExtension.Core.EventManagement;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Mappings;
using FrameworkExtension.Core.Repositories;
using FrameworkExtension.Core.Test.EntityFramework.Mapping;
using FrameworkExtension.Core.Test.Properties;
using FrameworkExtension.Core.Test.TestDomain;
using FrameworkExtension.Core.Test.TestQueries;
using MSTest.AssertionHelpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    [TestClass]
    public class Given_A_Generic_Repository
    {
        private static IWindsorContainer container;

        [ClassInitialize]
        public static void SetupClass(TestContext context)
        {
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDataContext>().ImplementedBy<EntityFrameworkTestContext>().DependsOn(new { connectionString = Settings.Default.Connection }).LifestyleTransient(),
                               Component.For<MappingConfiguration>().ImplementedBy<TestMappingConfiguration>().LifestyleTransient());

        }
        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange
            var context = container.Resolve<IDataContext>();

            //Act
            var repository = new EntityFrameworkRepository(context);

            //Assert
            repository.Context.IsSameByReference(context);
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
            var target = new EntityFrameworkRepository(context);
            
            //Act
            var result = target.Find(new TestQuery());

            //Assert
            context.VerifyAllExpectations();
            var foo = result.First();
            foo.IsNotNull();
            foo.Id.IsEqual(1);
            foo.Name.IsEqual("Test");
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
            var target = new EntityFrameworkRepository(context);

            //Act
            var result = target.Get(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(1);
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
            var target = new EntityFrameworkRepository(context);

            //Act
            var result = target.Get(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(1);
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
            var target = new EntityFrameworkRepository(context);

            //Act
            var result = target.Find(new TestQuery()).FirstOrDefault();

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(foo);
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
            var target = new EntityFrameworkRepository(context);

            //Act
            var testCommand = new TestCommand();
            target.Execute(testCommand);

            //Assert
            context.VerifyAllExpectations();
            testCommand.Called.IsTrue();
        }
    }
}
