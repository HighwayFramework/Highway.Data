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
using FrameworkExtension.Core.QueryObjects;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    [TestClass]
    public class Given_A_Query_Object 
    {
        internal static IWindsorContainer container;

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
        public void When_Passing_To_A_Repository_Query_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var repository = new EntityFrameworkRepository(context);

            //Act
            repository.Find(new TestQuery());

            //Assert
            context.VerifyAllExpectations();

        }

        [TestMethod]
        public void When_Executed_Returns_An_IEnumerable_Of_Items()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var query = new TestQuery();


            //Act
            IEnumerable<Foo> items = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            items.IsNotNull();

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
            var query = new TestQuery();

            //Act
            var retVal = query.Skip(4).Take(1).Execute(context);


            //Assert
            retVal.First().IsSameByReference(targetFoo);
        }

        [TestMethod]
        public void When_Calling_Output_Sql_with_Context_It_Outputs_SQL()
        {
            //arrange
            var target = new TestQuery();

            var context = container.Resolve<IDataContext>();

            //act
            var sqlOutput = target.OutputSQLStatement(context);

            //assert
            sqlOutput.IsNotNull();
            sqlOutput.IsTrue(x => x.ToLowerInvariant().Contains("from"));

        }
    }
}