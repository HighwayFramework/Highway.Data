using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Context_Without_Configuration
    {
        [TestMethod]
        public void Should_Apply_Context_Configuration_On_Construction()
        {
            //Arrange
            var mockConfig = MockRepository.GenerateStrictMock<IContextConfiguration>();
            mockConfig.Expect(x => x.ConfigureContext(Arg<DbContext>.Is.Anything)).Repeat.Once();
            
            //Act
            var target = new DataContext("Test", new FooMappingConfiguration(), mockConfig);

            //Assert
            mockConfig.VerifyAllExpectations();
        }
    }
}