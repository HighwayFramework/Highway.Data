using System.Security.Cryptography.X509Certificates;
using Highway.Data.Rest.Contexts;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Rest.ExampleAPI.Tests
{
    [TestClass]
    public class TypeConfigurationTests
    {
        [TestMethod]
        public void ShouldAllowSimpleFluentConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>().WithRoute("values").WithKey("id");

            //assert
            Assert.AreEqual("values/{id}",type.Uri);
        }

        [TestMethod]
        public void ShouldAllowLambdaFluentConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>().WithRoute("values").WithKey(x => x.Id);

            //assert
            Assert.AreEqual("values/{id}", type.Uri);
        }

        [TestMethod]
        public void ShouldAllowConventionBasedConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>().WithKey(x => x.Id);

            //assert
            Assert.AreEqual("foos/{id}", type.Uri);
        }

        [TestMethod]
        public void ShouldAllConventionsBasedIdConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>();

            //assert
            Assert.AreEqual("foos/{id}", type.Uri);
        }

        [TestMethod]
        public void ShouldAllowForCustomConventions()
        {
            //arrange
            var target = new ModelBuilder(new TestConvention());
            //act
            var type = target.Entity<Foo>();

            //assert
            Assert.AreEqual("{Id}/Test", type.Uri);
        }

        [TestMethod]
        public void ShouldAllowForOverridableConvention()
        {
            //arrange
            var target = new ModelBuilder(new OverrideTestConvention());
            //act
            var type = target.Entity<Foo>();

            //assert
            Assert.AreEqual("{id}/id", type.Uri);
        }
    }
}