using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.Interfaces;
using MSTest.AssertionHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.Core.Test
{
    [TestClass]
    public class Given_A_Scalar_Object
    {
        [TestMethod]
        public void When_Passing_To_A_Repository_Scalar_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDbContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var repository = new EntityFrameworkRepository(context);

            //Act
            repository.Find(new TestQuery());

            //Assert
            context.VerifyAllExpectations();

        }

        [TestMethod]
        public void When_Executed_Returns_A_Single_Value()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDbContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var query = new ScalarTestQuery();


            //Act
            int result = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            result.IsEqual(0);

        }
    }
}