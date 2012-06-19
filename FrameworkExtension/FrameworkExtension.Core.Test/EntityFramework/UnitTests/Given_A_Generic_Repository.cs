using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Repositories;
using FrameworkExtension.Core.Test.TestDomain;
using FrameworkExtension.Core.Test.TestQueries;
using MSTest.AssertionHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

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
            var repository = EntityFrameworkRepository.Create<EntityFrameworkTestContext>();

            //Assert
            repository.Context.IsOfType<EntityFrameworkTestContext>();

        }

        [TestMethod]
        public void When_Created_With_Connection_String_The_Context_Receives_That_String()
        {
            //Arrange
	        
            //Act
            var repository = EntityFrameworkRepository.Create<EntityFrameworkTestContext>("Test");

            //Assert
            ((EntityFrameworkTestContext) repository.Context).ConnectionString.ShouldBe("Test");

        }

        [TestMethod]
        public void When_Given_A_Contructor_It_Should_Support_Dependency_Injection()
        {
            //Arrange
            var context = new EntityFrameworkTestContext();

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
            Action action = () => EntityFrameworkRepository.Create<EFFailureContext>("Test");
            
            //Assert
            ExceptionAssert.Throws<InvalidOperationException>(action);
        }

        [TestMethod]
        public void Should_Execute_Query_Objects()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                    {
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new EntityFrameworkRepository(context);
            
            //Act
            var result = target.Find(new TestQuery());

            //Assert
            context.VerifyAllExpectations();
            var foo = result.First();
            foo.IsNotNull();
            foo.Id.IsEqual(1);
            foo.Name.IsEqual("Test");
        }

        [TestMethod]
        public void Should_Execute_Scalar_Objects_That_Return_Values()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                    {
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new EntityFrameworkRepository(context);

            //Act
            var result = target.Get(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(1);
        }

        [TestMethod]
        public void Should_Execute_Scalar_Objects_That_Return_Objects()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                    {
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new EntityFrameworkRepository(context);

            //Act
            var result = target.Get(new ScalarIntTestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(1);
        }

        [TestMethod]
        public void Should_Execute_A_Query_Object_And_Pull_The_First_Object()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            var foo = new Foo() {Id = 1, Name = "Test"};
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                    {
                        foo
                    }.AsQueryable());
            var target = new EntityFrameworkRepository(context);

            //Act
            var result = target.Get(new TestQuery());

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(foo);
        }

        [TestMethod]
        public void Should_Execute_Commands_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(
                new List<Foo>
                    {
                        new Foo() {Id = 1, Name = "Test"}
                    }.AsQueryable());
            var target = new EntityFrameworkRepository(context);

            //Act
            var testCommand = new TestCommand();
            target.Execute(testCommand);

            //Assert
            context.VerifyAllExpectations();
            testCommand.Called.IsTrue();
        }
    }
}
