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

        [TestMethod]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            var items = context.AsQueryable<Foo>();

            //Assert
            items.Count().IsEqual(5);
        }

        [TestInitialize]
        private void Setup()
        {
            Database.SetInitializer(new EntityFrameworkIntializer());
            context = new EFTestContext(Settings.Default.Connection);
        }

        [TestMethod]
        public void When_Add_Is_Called_The_Object_Is_Commited_To_The_Database()
        {
            //Arrange
	        
            //Act
            context.Add(new Foo());

            //Assert
            context.AsQueryable<Foo>().Count().IsEqual(0);
        }
    }
}