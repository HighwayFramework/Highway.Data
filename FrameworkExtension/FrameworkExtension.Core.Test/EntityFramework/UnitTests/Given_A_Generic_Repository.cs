using System;
using MSTest.AssertionHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    [TestClass]
    public class Given_A_Generic_Repository
    {
        [TestMethod]
        public void When_Created__It_Creates_The_Proper_Context()
        {
            //Arrange 

            //act
            var repository = EntityFrameworkRepository.Create<EFTestContext>();

            //Assert
            repository.Context.IsOfType<EFTestContext>();

        }

        [TestMethod]
        public void When_Created_With_Connection_String_The_Context_Receives_That_String()
        {
            //Arrange
	        
            //Act
            var repository = EntityFrameworkRepository.Create<EFTestContext>("Test");

            //Assert
            ((EFTestContext) repository.Context).ConnectionString.ShouldBe("Test");

        }

        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange
            var context = new EFTestContext();

            //Act
            var repository = new EntityFrameworkRepository(context);

            //Assert
            repository.Context.IsSameByReference(context);
        }

        [TestMethod]
        public void When_Given_A_Connection_String_With_A_Class_That_Doesnt_Have_A_Constructor_It_Throws()
        {
            //Arrange
	        
            //Act
            ExceptionAssert.Throws<InvalidOperationException>(() => EntityFrameworkRepository.Create<EFFailureContext>("Test"));
            //Assert
        }
    }
}
