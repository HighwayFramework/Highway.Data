using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using FrameworkExtension.Core.EventManagement;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Test.TestDomain;
using FrameworkExtension.Core.Test.TestQueries;
using FrameworkExtension.EntityFramework.Mappings;
using FrameworkExtension.EntityFramework.Repositories;
using FrameworkExtension.EntityFramework.Tests.Mapping;
using FrameworkExtension.EntityFramework.Tests.Properties;
using MSTest.AssertionHelpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Scalar_Object
    {
        internal static IWindsorContainer container;

        [ClassInitialize]
        public static void SetupClass(TestContext context)
        {
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDataContext>().ImplementedBy<EntityFrameworkTestContext>().DependsOn(new { connectionString = Settings.Default.Connection }).LifestyleTransient(),
                               Component.For<IMappingConfiguration>().ImplementedBy<TestMappingConfiguration>().LifestyleTransient());

        }
        [TestMethod]
        public void When_Passing_To_A_Repository_Scalar_Object_Then_It_Executes_Against_Context()
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
        public void When_Executed_Returns_A_Single_Value()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var query = new ScalarIntTestQuery();


            //Act
            int result = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(0);

        }

        //[TestMethod]
        //public void When_Calling_Output_Sql_with_Context_It_Outputs_SQL()
        //{
        //    //arrange
        //    var target = new ScalarIntTestQuery();

        //    var context = container.Resolve<IDataContext>();

        //    //act
        //    var sqlOutput = target.OutputSQLStatement(context);

        //    //assert
        //    sqlOutput.IsNotNull();
        //    sqlOutput.IsTrue(x => x.ToLowerInvariant().Contains("from"));

        //}

    }
}