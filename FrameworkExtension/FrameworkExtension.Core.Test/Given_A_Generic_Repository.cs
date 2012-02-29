using System;
using System.Data.Entity;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MSTest.AssertionHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.Core.Test
{
    [TestClass]
    public class Given_A_Generic_Repository
    {
        [TestMethod]
        public void When_Created__It_Creates_The_Proper_Context()
        {
            //Arrange 

            //act
            var repository = new EntityFrameworkRepository<EFTestContext>();

            //Assert
            repository.Context.IsOfType<EFTestContext>();

        }

        [TestMethod]
        public void When_Created_With_Connection_String_The_Context_Receives_That_String()
        {
            //Arrange
	        
            //Act
            var repository = new EntityFrameworkRepository<EFTestContext>("Test");

            //Assert
            ((EFTestContext) repository.Context).ConnectionString.ShouldBe("Test");

        }

        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange
            var context = new EFTestContext();

            //Act
            var repository = new EntityFrameworkRepository<EFTestContext>(context);

            //Assert
            repository.Context.IsByReferenceSame(context);
        }

        [TestMethod]
        public void When_Given_A_Query_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDbContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var repository = new EntityFrameworkRepository<EFTestContext>(context);

            //Act
            repository.Find(new TestQuery());

            //Assert
            context.VerifyAllExpectations();

        }
    }

    public class EFTestContext : DbContext, IDbContext
    {
        public EFTestContext()
        {
            
        }

        public EFTestContext(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}
