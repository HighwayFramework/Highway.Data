using System.Data;
using System.Data.Entity;
using System.Linq;
using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.Test.EntityFramework.Initializer;
using FrameworkExtension.Core.Test.EntityFramework.UnitTests;
using FrameworkExtension.Core.Test.Properties;
using FrameworkExtension.Core.Test.TestDomain;
using MSTest.AssertionHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrameworkExtension.Core.Test.EntityFramework.IntegrationTests
{
    [TestClass]
    public class Given_A_EF_Context
    {
        private EFTestContext context;

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            var items = context.AsQueryable<Foo>();

            //Assert
            items.Count().IsEqual(5);
        }

        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer(new ForceDeleteInitializer(new EntityFrameworkIntializer()));
            context = new EFTestContext(Settings.Default.Connection);
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
            entry.State.IsEqual(EntityState.Added);
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
            entry.State.IsEqual(EntityState.Deleted);
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
            entry.State.IsEqual(EntityState.Detached);
        }
    }

    public static class TestCategories
    {
        public const string Database = "Database";
    }
}