using System.Collections.Generic;
using System.Linq;
using Highway.Data.Repositories;
using Highway.Data.Interfaces;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Highway.Data.Tests.TestQueries;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Scalar_Object : BaseTest<object>
    {
        [TestMethod]
        public void When_Passing_To_A_Repository_Scalar_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>())
                .Return(new List<Foo>() { new Foo() }.AsQueryable())
                .Repeat.Once();
            var repository = new Repository(context);

            //Act
            repository.Get(new ScalarFooTestQuery());

            //Assert
            context.VerifyAllExpectations();

        }

        [TestMethod]
        public void When_Executed_Returns_A_Single_Value()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>())
                .Return(new List<Foo>().AsQueryable())
                .Repeat.Once();
            var query = new ScalarIntTestQuery();


            //Act
            int result = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            result.ShouldBe(0);

        }

        //[TestMethod]
        //public void When_Calling_Output_Sql_with_Context_It_Outputs_SQL()
        //{
        //    //arrange
        //    var target = new ScalarIntTestQuery();

        //    var context = container.Resolve<IDataContext>();

        //    //act
        //    var sqlOutput = target.OutputSQLStatement(context);

        //    //assert
        //    sqlOutput.IsNotNull();
        //    sqlOutput.IsTrue(x => x.ToLowerInvariant().Contains("from"));

        //}

    }
}