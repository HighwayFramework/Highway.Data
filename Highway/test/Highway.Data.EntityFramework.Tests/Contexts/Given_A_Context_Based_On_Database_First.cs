using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Tests.Initializer;
using Highway.Data.EntityFramework.Tests.TestContexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Context_Based_On_Database_First
    {
        [TestMethod]
        public void Should_Not_Call_Code_First_Bindings()
        {
            //Arrange
            Database.SetInitializer(new HighwayTestInitializer<DataContext>(c =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        c.Add(new Member());
                    }
                }));
            var context = new DataContext("name=TDDAirEntities");

            //Act
            var members = context.AsQueryable<Member>().ToList();

            //Assert
        }
    }
}