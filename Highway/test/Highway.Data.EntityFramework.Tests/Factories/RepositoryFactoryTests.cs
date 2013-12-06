#region

using FluentAssertions;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

#endregion

namespace Highway.Data.EntityFramework.Tests.Factories
{
    [TestClass]
    public class RepositoryFactoryTests
    {
        [TestMethod]
        public void ShouldCreateRepository()
        {
            // arrange
            IRepositoryFactory factory = new RepositoryFactory(new []{new TestDomain()});

            // act
            IRepository repo = factory.Create<TestDomain>();

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateRepositoryFromType()
        {
            // arrange
            IRepositoryFactory factory = new RepositoryFactory(new[] { new TestDomain() });

            // act
            IRepository repo = factory.Create(typeof (TestDomain));

            // assert
            repo.Should().NotBeNull();
        }
    }
}