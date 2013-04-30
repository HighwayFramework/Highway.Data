
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.EventManagement;
using Highway.Data;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;

namespace Highway.Data.RavenDB.Tests.IntegrationTests
{
    [TestClass]
    public class Given_A_EF_Context : ContainerTest<DataContext>
    {
        public override DataContext ResolveTarget()
        {
            return Container.Resolve<DataContext>();
        }

        public override void RegisterComponents(IWindsorContainer container)
        {
            var embeddableDocumentStore = new EmbeddableDocumentStore()
            {
                DataDirectory = "",
                RunInMemory = true
            };
            embeddableDocumentStore.Initialize();
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<IDocumentStore>().Instance(embeddableDocumentStore),
                               Component.For<IDocumentSession>().Instance(embeddableDocumentStore.OpenSession()),
                               Component.For<ILog>().ImplementedBy<NoOpLogger>());
            base.RegisterComponents(container);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            var session = ResolveTarget();
            for (int i = 0; i < 5; i++)
            {
                session.Store(new Foo());
            }
            session.SaveChanges();
            target.AsQueryable<Foo>().ToList();
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            IQueryable<Foo> items = target.AsQueryable<Foo>();

            //Assert
            items.Count().ShouldBe(5);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Add_Called_Object_Is_Added_To_The_Database()
        {
            //Arrange

            //Act
            var item = new Foo();
            target.Add(item);
            target.SaveChanges();
            IQueryable<Foo> items = target.AsQueryable<Foo>();

            //Assert
            items.Count().ShouldBe(5);

            target.Remove(item);
            target.SaveChanges();
        }
    }
}