using System.Data;
using System.Data.Entity;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using Highway.Data.EntityFramework.Tests.Initializer;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;
using Highway.Test.MSTest;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestContext = Highway.Data.EntityFramework.Tests.UnitTests.TestContext;

namespace Highway.Data.EntityFramework.Tests.IntegrationTests
{
    [TestClass]
    public class Given_A_EF_Context
    {
        private TestContext context;
        private static IWindsorContainer container;

        [ClassInitialize]
        public static void SetupClass(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>().LifestyleTransient(),
                               Component.For<TestContext>().ImplementedBy<TestContext>().DependsOn(new { connectionString = Settings.Default.Connection }).LifestyleTransient(),
                               Component.For<IMappingConfiguration>().ImplementedBy<TestMappingConfiguration>().LifestyleTransient());

        }

        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer(new ForceDeleteInitializer(new EntityFrameworkIntializer()));
            context = container.Resolve<TestContext>();
        }


        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            var items = context.AsQueryable<Foo>();

            //Assert
            items.Count().ShouldBe(5);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Add_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_An_Added_State()
        {
            //Arrange
	        
            //Act
            var item = new Foo();
            context.Add(item);

            //Assert
            context.ChangeTracker.DetectChanges();
            var entry = context.Entry(item);
            entry.State.ShouldBe(EntityState.Added);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Remove_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_A_Deleted_State()
        {
            //Arrange
	
            //Act
            var item = context.AsQueryable<Foo>().First();
            context.Remove(item);

            //Assert
            context.ChangeTracker.DetectChanges();
            var entry = context.Entry(item);
            entry.State.ShouldBe(EntityState.Deleted);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Detach_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_A_Detached_State()
        {
            //Arrange
	
            //Act
            var item = context.AsQueryable<Foo>().First();
            context.Detach(item);

            //Assert
            context.ChangeTracker.DetectChanges();
            var entry = context.Entry(item);
            entry.State.ShouldBe(EntityState.Detached);
        }
    }

    public static class TestCategories
    {
        public const string Database = "Database";
    }
}