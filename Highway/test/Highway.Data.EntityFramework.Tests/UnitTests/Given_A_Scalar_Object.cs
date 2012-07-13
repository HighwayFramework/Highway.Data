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
    public class Given_A_Scalar_Object
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
        public void When_Passing_To_A_Repository_Scalar_Object_Then_It_Executes_Against_Context()
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
            result.ShouldBe(0);

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