#region

using FluentAssertions;
using Highway.Data.Domain;
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
            var contextFactory = MockRepository.GenerateMock<IContextFactory>();
            IRepositoryFactory factory = new RepositoryFactory(contextFactory);

            // act
            IRepository repo = factory.Create<FooDomain>();

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateRepositoryFromType()
        {
            // arrange
            var contextFactory = MockRepository.GenerateMock<IContextFactory>();
            IRepositoryFactory factory = new RepositoryFactory(contextFactory);

            // act
            IRepository repo = factory.Create(typeof (FooDomain));

            // assert
            repo.Should().NotBeNull();
        }
    }
}