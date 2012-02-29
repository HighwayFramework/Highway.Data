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
            repository.Context.IsByReferenceSame(context);
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

    public class EFFailureContext : IDbContext
    {
        public IQueryable<T> AsQueryable<T>()
        {
            throw new NotImplementedException();
        }
    }

    public class Foo
    {
    }

    public class TestQuery : IQuery<Foo>
    {
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

        public IQueryable<T> AsQueryable<T>()
        {
            return null;
        }
    }
}
