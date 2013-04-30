using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using CommonServiceLocator.WindsorAdapter;
using Highway.Data.EventManagement;
using Highway.Data;
using Highway.Data.RavenDB.Tests.TestQueries;
using Highway.Data.Tests;
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
    public class Given_A_Scalar_Object : BaseTest<object>
    {
        private static IWindsorContainer _container;

        [ClassInitialize]
        public static void SetupClass(TestContext context)
        {
            _container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));
            _container.Kernel.Resolver.AddSubResolver(new ArrayResolver(_container.Kernel));
            var embeddableDocumentStore = new EmbeddableDocumentStore()
            {
                DataDirectory = "",
                RunInMemory = true
            };
            embeddableDocumentStore.Initialize();
            _container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDataContext>().ImplementedBy<DataContext>().LifestyleTransient(),
                               Component.For<IDocumentStore>().Instance(embeddableDocumentStore),
                               Component.For<IDocumentSession>().Instance(embeddableDocumentStore.OpenSession()),
                               Component.For<ILog>().ImplementedBy<NoOpLogger>());
        }


        [TestMethod]
        public void When_Passing_To_A_Repository_Scalar_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>())
                .Return(new List<Foo> {new Foo()}.AsQueryable())
                .Repeat.Once();
            var repository = new Repository(context);

            //Act
            repository.Find(new ScalarFooTestQuery());

            //Assert
            context.VerifyAllExpectations();
        }

        [TestMethod]
        public void When_Executed_Returns_A_Single_Value()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>())
                .Return(new List<Foo>().AsQueryable())
                .Repeat.Once();
            var query = new ScalarIntTestQuery();


            //Act
            int result = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            result.ShouldBe(0);
        }
    }
}