#region

using System.Data.Entity;
using Common.Logging;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

#endregion

namespace Highway.Data.EntityFramework.Tests.Logging
{
    [TestClass]
    public class Given_A_Context_With_a_Logger
    {
        [TestMethod, TestCategory("Database")]
        public void Should_Add_log_messages_for_add()
        {
            //Arrange
            Database.SetInitializer(new DropCreateInitializer<TestDataContext>());
            var logger = MockRepository.GenerateMock<ILog>();
            logger.Expect(x => x.TraceFormat(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).IgnoreArguments().
                Repeat.Once();
            logger.Expect(x => x.DebugFormat(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).IgnoreArguments().
                Repeat.Once();
            var context = new TestDataContext(Settings.Default.Connection, new FooMappingConfiguration(), logger);

            //Act
            context.Add(new Foo());

            //Assert
            logger.VerifyAllExpectations();
        }
    }
}