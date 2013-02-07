using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.PrebuiltQueries;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_a_GetById_Query
    {
        [TestMethod]
        public void ShouldReturnFoo()
        {
            //Arrange
            var context = new TestDataContext(Settings.Default.Connection, new FooMappingConfiguration(),
                                              new ConsoleOutLogger("Test", LogLevel.All, true, true, true, string.Empty));

            var item = new Foo();
            context.Add(item);
            context.Commit();

            var target = Queries.GetById<Foo>(item.Id);

            //Act
            var result = target.Execute(context);

            //Assert
            Assert.AreEqual(item.Id, result.Id);
            context.Remove(result);
            context.Commit();
        }

        [TestMethod]
        public void ShouldReturnAllFoos()
        {
            //Arrange
            var context = new TestDataContext(Settings.Default.Connection, new FooMappingConfiguration(),
                                             new ConsoleOutLogger("Test", LogLevel.All, true, true, true, string.Empty));

            var item = new Foo();
            var item2 = new Foo();
            context.Add(item);
            context.Add(item2);
            context.Commit();
            var expectedCount = context.AsQueryable<Foo>().Count();

            var target = Queries.FindAll<Foo>();

            //Act
            var results = target.Execute(context);

            //Assert
            Assert.AreEqual(results.Count(), expectedCount);
            context.Remove(item);
            context.Remove(item2);
            context.Commit();
        }
    }
}