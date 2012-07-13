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
using Highway.Data.QueryObjects;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Query_Object 
    {
        internal static IWindsorContainer container;

        [ClassInitialize]
        public static void SetupClass(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDataContext>().ImplementedBy<TestContext>().DependsOn(new { connectionString = Settings.Default.Connection }).LifestyleTransient(),
                               Component.For<IMappingConfiguration>().ImplementedBy<TestMappingConfiguration>().LifestyleTransient());

        }

        [TestMethod]
        public void When_Passing_To_A_Repository_Query_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var repository = new Repository(context);

            //Act
            repository.Find(new FindFoo());

            //Assert
            context.VerifyAllExpectations();

        }

        [TestMethod]
        public void When_Executed_Returns_An_IEnumerable_Of_Items()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var query = new FindFoo();


            //Act
            IEnumerable<Foo> items = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            items.ShouldNotBeNull();

        }

        [TestMethod]
        public void When_Paging_Should_Affect_The_Base_Query_Before_It_Is_Executed()
        {
            //Arrange
            var targetFoo = new Foo();
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>()
                {
                    new Foo(),
                    new Foo(),
                    new Foo(),
                    new Foo(),
                    targetFoo
                }.AsQueryable()).Repeat.Once();
            var query = new FindFoo();

            //Act
            var retVal = query.Skip(4).Take(1).Execute(context);


            //Assert
            retVal.First().ShouldBeSame(targetFoo);
        }

        [TestMethod]
        public void When_Calling_Output_Sql_with_Context_It_Outputs_SQL()
        {
            //arrange
            var target = new FindFoo();

            var context = container.Resolve<IDataContext>();

            //act
            var sqlOutput = target.OutputSQLStatement(context);

            //assert
            sqlOutput.ShouldNotBeNull();
            sqlOutput.ShouldContain("from");

        }
    }
}