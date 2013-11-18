#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Tests.UtiltiyTests
{
    [TestClass]
    public class DynamicQueryableTests : BaseCollectionTests
    {
        [TestMethod]
        public void ShouldFilterByString()
        {
            //Act
            var results = items.AsQueryable().Where("Name = @0", "Devlin");

            //Assert
            Assert.IsTrue(results.Count() == 1);
            Assert.AreEqual("Devlin", results.Single().Name);
        }

        [TestMethod]
        public void ShouldSelectByString()
        {
            //Act
            IEnumerable results = items.AsQueryable().Select("new(Name as FirstName)");

            //Assert
            Assert.IsTrue(results.Count() == 3);
            Assert.AreEqual("Devlin", results.Cast<dynamic>().First().FirstName);
        }
    }

    public class BaseCollectionTests
    {
        public ICollection<Foo> items = new Collection<Foo>
        {
            new Foo {Name = "Devlin"},
            new Foo {Name = "Tim"},
            new Foo {Name = "Allen"}
        };
    }

    [TestClass]
    public class DynamicEnumerableTests : BaseCollectionTests
    {
        [TestMethod]
        public void ShouldFilterByString()
        {
            //Act
            var results = items.Where("Name = @0", "Devlin");

            //Assert
            Assert.IsTrue(results.Count() == 1);
            Assert.AreEqual("Devlin", results.Single().Name);
        }

        [TestMethod]
        public void ShouldSelectByString()
        {
            //Act
            IEnumerable results = items.Select("new(Name as FirstName)");

            //Assert
            Assert.IsTrue(results.Count() == 3);
            Assert.AreEqual("Devlin", results.Cast<dynamic>().First().FirstName);
        }
    }
}