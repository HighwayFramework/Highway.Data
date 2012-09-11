using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_An_Async_Repository
    {
        [TestMethod]
        public void Should_Not_Execute_Until_Needed()
        {
            ////Arrange
            //var mockContext = MockRepository.GenerateStub<IDataContext>();
            //var expected = new Foo() {Id = 1, Name = "Test"};
            //mockContext.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo> {expected}.AsQueryable());
            //var repo = new AsyncRepository(mockContext);

            ////Act
            //var result = repo.Find(new FindFoo());
            //mockContext.AssertWasNotCalled(x => x.AsQueryable<Foo>());
            //var testReuslt = result.Result;

            ////Assert
            //testReuslt.First().ShouldBe(expected);
        }
    }
}