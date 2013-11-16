using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Tests.TestContexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.Contexts
{
    [TestClass]
    public class DatabaseFirstContextTests
    {
        [TestMethod]
        public void Should_Not_Call_Code_First_Bindings()
        {
            //Arrange
            Database.SetInitializer(new DropCreateInitializer<DataContext>(c =>
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
            Assert.IsTrue(members.Count() >= 5);
        }
    }
}