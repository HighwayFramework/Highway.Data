using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;
using Highway.Data.Tests.TestQueries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.Performance
{
    [TestClass]
    public class Given_A_Query_Can_Performance_Test_It
    {
        [TestMethod]
        public void Should_Throw_An_Error_When_Query_Is_To_Slow()
        {
            //Arrange
            var consoleOutLogger = new ConsoleOutLogger("Performance", LogLevel.All, true, true, true, "");
            var context = new TestDataContext(Settings.Default.Connection, new FooMappingConfiguration(),
                                              consoleOutLogger);

            //Act
            IEnumerable<Foo> result = null;
            try
            {
                result = new FindFoo().RunPerformanceTest(context, consoleOutLogger, maxAllowableMilliseconds: -1);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Query FindFoo"));
            }


            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void Should_Execute_Query_With_Performance_Info()
        {
            //Arrange
            var consoleOutLogger = new ConsoleOutLogger("Performance", LogLevel.All, true, true, true, "");
            var context = new TestDataContext(Settings.Default.Connection, new FooMappingConfiguration(),
                                              consoleOutLogger);
            List<Foo> nullResult = context.AsQueryable<Foo>().ToList();

            //Act
            IEnumerable<Foo> result = null;

            result = new FindFoo().RunPerformanceTest(context, consoleOutLogger, false);


            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_Run_Compilation_Test_For_Context()
        {
            //Arrange
            var consoleOutLogger = new ConsoleOutLogger("Performance", LogLevel.All, true, true, true, "");
            var context = new TestDataContext(Settings.Default.Connection, new FooMappingConfiguration(),
                                              consoleOutLogger);

            //Act
            IEnumerable<Foo> result = null;
            try
            {
                var query = new FindFoo();
                context.RunStartUpPerformanceTest(query, consoleOutLogger, maxAllowableMilliseconds: -1);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Context Compilation in"));
            }


            //Assert
            Assert.IsNull(result);
        }
    }
}