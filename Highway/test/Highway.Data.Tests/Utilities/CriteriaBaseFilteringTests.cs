#region

using System.Collections.Generic;
using System.Linq;
using Highway.Data.Filtering;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.Tests.Utilities
{
    [TestClass]
    public class CriteriaBaseFilteringTests
    {
        [TestMethod]
        public void CanFilterWithABasicCriteria()
        {
            //arrange
            var expectedItem = new Foo {Name = "Devlin", Id = 16};
            var items = new List<Foo>
            {
                new Foo {Name = "Test", Id = 15},
                expectedItem
            };

            //act
            IEnumerable<Foo> results = items.FilterBy(Criteria.Field<string>("Name").IsEqualTo("Devlin"));

            //assert
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedItem, results.Single());
        }

        [TestMethod]
        public void CanFilterWithAnAndCriteria()
        {
            //arrange
            var expectedItem = new Foo {Name = "Devlin", Id = 16, Address = "Testing"};
            var items = new List<Foo>
            {
                new Foo {Name = "Test", Id = 15},
                new Foo {Name = "Devlin", Id = 10, Address = "Not the one"},
                expectedItem
            };

            //act
            IEnumerable<Foo> results = items.FilterBy(Criteria.Field<string>("Name").IsEqualTo("Devlin")
                .And(Criteria.Field<string>("Address")
                    .IsEqualTo("Testing")));

            //assert
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedItem, results.Single());
        }

        [TestMethod]
        public void CanFilterWithAnOrCriteria()
        {
            //arrange
            var expectedItem = new Foo {Name = "Devlin", Id = 16, Address = "Testing"};
            var expectedItemTwo = new Foo {Name = "Tim", Id = 10, Address = "Not the one"};
            var items = new List<Foo>
            {
                new Foo {Name = "Test", Id = 15},
                expectedItemTwo,
                expectedItem
            };

            //act
            IEnumerable<Foo> results = items.FilterBy(Criteria.Field<string>("Name").IsEqualTo("Devlin")
                .Or(Criteria.Field<string>("Name")
                    .IsEqualTo("Tim")));

            //assert
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(expectedItem, results.Single(x => x.Name == "Devlin"));
            Assert.AreEqual(expectedItemTwo, results.Single(x => x.Name == "Tim"));
        }
    }
}