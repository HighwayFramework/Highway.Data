#region

using System;
using FluentAssertions;
using Highway.Data.Rest.Configuration.Conventions;
using Highway.Data.Rest.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Rest.Tests
{
    [TestClass]
    public class ModelBuilderTests
    {
        [TestMethod]
        public void ShouldCompileEmptyList()
        {
            //arrange
            var target = new ModelBuilder(new DefaultConvention());

            //act
            var result = target.Compile();

            //assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldCompileADefinitionForSingleEntity()
        {
            //arrange
            var target = new ModelBuilder(new DefaultConvention());
            target.Entity<Foo>();

            //act
            var result = target.Compile();

            //assert
            result.Count.Should().Be(1);
            result.ContainsKey(typeof (Foo)).Should().BeTrue();
            result[typeof (Foo)].AllUri.Should().Be("foos");
        }

        [TestMethod]
        public void ShouldCompileADefinitionForManyEntities()
        {
            //arrange
            var target = new ModelBuilder(new DefaultConvention());
            target.Entity<Foo>();
            target.Entity<Bar>();
            target.Entity<Baz>();
            target.Entity<Qux>();

            //act
            var result = target.Compile();

            //assert
            result.Count.Should().Be(4);
            result.ContainsKey(typeof (Foo)).Should().BeTrue();
            result[typeof (Foo)].AllUri.Should().Be("foos");
            result.ContainsKey(typeof (Bar)).Should().BeTrue();
            result[typeof (Bar)].AllUri.Should().Be("bars");
            result.ContainsKey(typeof (Baz)).Should().BeTrue();
            result[typeof (Baz)].AllUri.Should().Be("bazzes");
            result.ContainsKey(typeof (Qux)).Should().BeTrue();
            result[typeof (Qux)].AllUri.Should().Be("quxes");
        }
    }

    public class Foo
    {
        public int Id { get; set; }

        public int Test()
        {
            throw new NotImplementedException();
        }
    }

    public class Bar
    {
        public int Id { get; set; }
    }

    public class Baz
    {
        public int Id { get; set; }
    }

    public class Qux
    {
        public int Id { get; set; }
    }
}