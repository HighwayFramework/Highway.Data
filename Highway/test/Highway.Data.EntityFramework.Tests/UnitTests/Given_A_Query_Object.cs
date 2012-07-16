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
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Query_Object : AutoMockingTest<FindFoo>
    {
        private IDataContext mockContext;

        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(
                Component.For<IEventManager>()
                    .ImplementedBy<EventManager>(),
                Component.For<IRepository>()
                    .ImplementedBy<Repository>(),
                Component.For<IMappingConfiguration>()
                    .ImplementedBy<FooMappingConfiguration>());
            base.RegisterComponents(container);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            mockContext = Container.Resolve<IDataContext>();
        }

        [TestMethod]
        public void When_Passing_To_A_Repository_Query_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            mockContext.Expect(x => x.AsQueryable<Foo>())
                .Return(new List<Foo>().AsQueryable())
                .Repeat.Once();
            var repo = Container.Resolve<IRepository>();

            //Act
            repo.Find(new FindFoo());

            //Assert
            mockContext.VerifyAllExpectations();

        }

        [TestMethod]
        public void When_Executed_Returns_An_IEnumerable_Of_Items()
        {
            //Arrange
            mockContext.Expect(x => x.AsQueryable<Foo>())
                .Return(new List<Foo>().AsQueryable())
                .Repeat.Once();

            //Act
            IEnumerable<Foo> items = target.Execute(mockContext);

            //Assert
            mockContext.VerifyAllExpectations();
            items.ShouldNotBeNull();

        }

        [TestMethod]
        public void When_Paging_Should_Affect_The_Base_Query_Before_It_Is_Executed()
        {
            //Arrange
            var targetFoo = new Foo();
            mockContext.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>()
                {
                    new Foo(),
                    new Foo(),
                    new Foo(),
                    new Foo(),
                    targetFoo
                }.AsQueryable()).Repeat.Once();

            //Act
            var retVal = target.Skip(4).Take(1).Execute(mockContext);


            //Assert
            retVal.First().ShouldBeSame(targetFoo);
        }

        [TestMethod]
        public void When_Calling_Output_Sql_with_Context_It_Outputs_SQL()
        {
            //arrange
            var target = new FindFoo();

            var context = new TestDataContext(Settings.Default.Connection, new IMappingConfiguration[] { new FooMappingConfiguration() });

            //act
            var sqlOutput = target.OutputSQLStatement(context);

            //assert
            sqlOutput.ShouldNotBeNull();
            sqlOutput.ShouldContain("from");

        }
    }
}