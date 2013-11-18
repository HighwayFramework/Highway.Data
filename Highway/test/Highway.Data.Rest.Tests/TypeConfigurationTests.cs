#region

using System;
using System.Data;
using FluentAssertions;
using Highway.Data.Rest.Configuration.Entities;
using Highway.Data.Rest.Configuration.Interfaces;
using Highway.Data.Rest.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Rest.Tests
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
            var type = target.Entity<Foo>().WithRoute("values").WithKey("Id");

            //assert
            Assert.AreEqual("values/{Id}", type.SingleUri);
        }

        [TestMethod]
        public void ShouldAllowLambdaFluentConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>().WithRoute("values").WithKey(x => x.Id);

            //assert
            Assert.AreEqual("values/{id}", type.SingleUri);
        }

        [TestMethod]
        public void ShouldAllowConventionBasedConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>().WithKey(x => x.Id);

            //assert
            Assert.AreEqual("foos/{id}", type.SingleUri);
        }

        [TestMethod]
        public void ShouldAllConventionsBasedIdConfiguration()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            var type = target.Entity<Foo>();

            //assert
            Assert.AreEqual("foos/{id}", type.SingleUri);
        }

        [TestMethod]
        public void ShouldAllowForCustomConventions()
        {
            //arrange
            var target = new ModelBuilder(new TestConvention());
            //act
            var type = target.Entity<Foo>();

            //assert
            type.SingleUri.Should().Be("{Id}/Test");
            type.AllUri.Should().Be("{Id}");
        }

        [TestMethod]
        public void ShouldAllowForOverridableConvention()
        {
            //arrange
            var target = new ModelBuilder(new OverrideTestConvention());
            //act
            var type = target.Entity<Foo>();

            //assert
            type.SingleUri.Should().Be("{id}/id");
            type.AllUri.Should().Be("{id}");
        }

        [TestMethod]
        public void ShouldNotAllowMethodSignaturesForKeys()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            Action act = () => target.Entity<Foo>().WithKey(x => x.Test());

            //assert
            act.ShouldThrow<InvalidExpressionException>();
        }

        [TestMethod]
        public void SettingStringKeyShouldPopulateKeySelector()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            RestTypeConfiguration<Foo> restTypeConfiguration = target.Entity<Foo>();
            IRestTypeDefinition typeDefinition = (IRestTypeDefinition) restTypeConfiguration.WithKey("Id");
            ;

            //assert
            Assert.IsNotNull(typeDefinition.KeyProperty);
        }

        [TestMethod]
        public void SettingStringKeyToInvalidPropertyShouldThrowException()
        {
            //arrange
            var target = new ModelBuilder();

            //act
            RestTypeConfiguration<Foo> restTypeConfiguration = target.Entity<Foo>();
            Action act = () => restTypeConfiguration.WithKey("id");

            //assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}