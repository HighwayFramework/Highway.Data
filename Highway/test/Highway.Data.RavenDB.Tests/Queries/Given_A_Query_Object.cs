using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using CommonServiceLocator.WindsorAdapter;
using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using Highway.Data.QueryObjects;
using Highway.Data.RavenDB.Tests.TestQueries;
using Highway.Data.Tests.TestDomain;
using Highway.Test.MSTest;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
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
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            var embeddableDocumentStore = new EmbeddableDocumentStore()
                {
                    DataDirectory = "", RunInMemory = true
                };
            embeddableDocumentStore.Initialize();
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDataContext>().ImplementedBy<DataContext>().LifestyleTransient(),
                               Component.For<IDocumentStore>().Instance(embeddableDocumentStore),
                               Component.For<IDocumentSession>().Instance(embeddableDocumentStore.OpenSession()),
                               Component.For<ILog>().ImplementedBy<NoOpLogger>());
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
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>
                {
                    new Foo(),
                    new Foo(),
                    new Foo(),
                    new Foo(),
                    targetFoo
                }.AsQueryable()).Repeat.Once();
            var query = new FindFoo();

            //Act
            IEnumerable<Foo> retVal = query.Skip(4).Take(1).Execute(context);


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
            string sqlOutput = target.OutputSQLStatement(context);

            //assert
            sqlOutput.ShouldNotBeNull();
            sqlOutput.ShouldContain("from");
        }
    }
}