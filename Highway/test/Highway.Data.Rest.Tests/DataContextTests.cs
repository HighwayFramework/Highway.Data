#region

using Highway.Data.Rest.Configuration;
using Highway.Data.Rest.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Rest.Tests
{
    [TestClass]
    public class DataContextTests
    {
        [TestMethod]
        public void ShouldConnectToHttpForFooUri()
        {
            ////arrange
            //var mockHttpCommunication = MockRepository.GenerateMock<IHttpClientAdapter>();
            //mockHttpCommunication.Expect(x => x.All()).Return(new List<Foo>() { new Foo() { Id = 12 } }.ToJson());
            //var config = new DefaultContextConfiguration("http://www.test.com");
            //var target = new DataContext(new FooRestMapping(), mockHttpCommunication, config);

            ////act
            //var results = target.AsQueryable<Foo>();

            ////assert
            //results.Count().Should().Be(1);
            //results.Count(x => x.Id == 12).Should().Be(1);
        }
    }

    public class FooRestMapping : IMappingConfiguration
    {
        public ModelBuilder Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Foo>().WithRoute("foo").WithKey(x => x.Id);
            return modelBuilder;
        }
    }
}