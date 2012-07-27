using System.Data;
using System.Data.Entity;
using System.Linq;
using Castle.Windsor;
using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using Highway.Data.EntityFramework.Tests.Initializer;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Component = Castle.MicroKernel.Registration.Component;
using Highway.Data.EntityFramework.Tests.Properties;

namespace Highway.Data.EntityFramework.Tests.IntegrationTests
{
    [TestClass]
    public class Given_A_EF_Context : ContainerTest<TestDataContext>
    {
        public override TestDataContext ResolveTarget()
        {
            return Container.Resolve<TestDataContext>(new { connectionString = Settings.Default.Connection });
        }
        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<IEventManager>().ImplementedBy<EventManager>(),
                               Component.For<IMappingConfiguration>().ImplementedBy<FooMappingConfiguration>());

            base.RegisterComponents(container);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            Database.SetInitializer(new ForceDeleteInitializer(new EntityFrameworkIntializer()));
            target.AsQueryable<Foo>().ToList();
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            var items = target.AsQueryable<Foo>();

            //Assert
            items.Count().ShouldBe(5);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Add_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_An_Added_State()
        {
            //Arrange
            var item = new Foo();

            //Act
            target.Add(item);

            //Assert
            target.ChangeTracker.DetectChanges();
            var entry = target.Entry(item);
            entry.State.ShouldBe(EntityState.Added);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Remove_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_A_Deleted_State()
        {
            //Arrange
	
            //Act
            var item = target.AsQueryable<Foo>().First();
            target.Remove(item);

            //Assert
            target.ChangeTracker.DetectChanges();
            var entry = target.Entry(item);
            entry.State.ShouldBe(EntityState.Deleted);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Detach_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_A_Detached_State()
        {
            //Arrange
	
            //Act
            var item = target.AsQueryable<Foo>().First();
            target.Detach(item);

            //Assert
            target.ChangeTracker.DetectChanges();
            var entry = target.Entry(item);
            entry.State.ShouldBe(EntityState.Detached);
            target.Dispose();
        }
    }
}