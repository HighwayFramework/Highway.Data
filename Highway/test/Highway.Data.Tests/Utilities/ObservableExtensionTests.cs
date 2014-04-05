using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.Utilities
{
    [TestClass]
    public class ObservableExtensionTests
    {
        [TestMethod]
        public void ShouldConvertListToObservable()
        {
            //Arrange
            var items = new List<Foo>()
            {
                new Foo() {Name = "Test"},
                new Foo() {Name = "Test2"}
            };

            //Act
            var result = items.ToObservableList();

            //Assert
            result.Should().BeAssignableTo<ObservableCollection<Foo>>();
            result.ShouldAllBeEquivalentTo(items);
        }
    }
}