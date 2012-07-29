using System.Collections.Generic;
using System.Linq;
using Highway.Data.Interfaces;
using Highway.Data.Repositories;
using Highway.Data.Tests.TestDomain;
using Highway.Data.Tests.TestQueries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_An_Async_Repository
    {
        [TestMethod]
        public void Should_Not_Execute_Until_Needed()
        {
            //Arrange
            var mockContext = MockRepository.GenerateStrictMock<IDataContext>();
            mockContext.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo> {new Foo() {Id = 1, Name = "Test"}}.AsQueryable());
            var repo = new AsyncRepository(mockContext);

            //Act
            var result = repo.Find(new FindFoo());
            mockContext.AssertWasNotCalled(x=>x.AsQueryable<Foo>());
            result.Start();
            
            //Assert
            
            mockContext.VerifyAllExpectations();

        }
    }
}