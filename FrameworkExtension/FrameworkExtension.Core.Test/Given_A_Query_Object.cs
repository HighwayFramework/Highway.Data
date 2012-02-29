using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MSTest.AssertionHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.Core.Test
{
    [TestClass]
    public class Given_A_Query_Object
    {
        [TestMethod]
        public void When_Passing_To_A_Repository_Query_Object_Then_It_Executes_Against_Context()
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
        public void When_Executed_Returns_An_IEnumerable_Of_Items()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDbContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var query = new TestQuery();


            //Act
            IEnumerable<Foo> items = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            items.IsNotNull();

        }
    }
}