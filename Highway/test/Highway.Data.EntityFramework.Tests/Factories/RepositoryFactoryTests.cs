#region

using Common.Logging.Simple;
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
            IDomainRepositoryFactory factory = new DomainRepositoryFactory(new []{new TestDomain()});

            // act
            IRepository repo = factory.Create<TestDomain>();

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateRepositoryFromType()
        {
            // arrange
            IDomainRepositoryFactory factory = new DomainRepositoryFactory(new[] { new TestDomain() });

            // act
            IRepository repo = factory.Create(typeof (TestDomain));

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateSimpleRepositoryWithAllParams()
        {
            //Arrange
            var testDomain = new TestDomain();
            IRepositoryFactory factory = new RepositoryFactory(testDomain.ConnectionString,testDomain.Mappings, testDomain.Context, new NoOpLogger());

            //Act
            IRepository repo = factory.Create();

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateSimpleRepositoryWithContextNoLog()
        {
            //Arrange
            var testDomain = new TestDomain();
            IRepositoryFactory factory = new RepositoryFactory(testDomain.ConnectionString, testDomain.Mappings, testDomain.Context);

            //Act
            IRepository repo = factory.Create();

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateSimpleRepositoryWithLogNoContext()
        {
            //Arrange
            var testDomain = new TestDomain();
            IRepositoryFactory factory = new RepositoryFactory(testDomain.ConnectionString, testDomain.Mappings, testDomain.Context, new NoOpLogger());

            //Act
            IRepository repo = factory.Create();

            // assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateSimpleRepositoryWithMinimum()
        {
            //Arrange
            var testDomain = new TestDomain();
            IRepositoryFactory factory = new RepositoryFactory(testDomain.ConnectionString, testDomain.Mappings, new NoOpLogger());

            //Act
            IRepository repo = factory.Create();

            // assert
            repo.Should().NotBeNull();
        }
    }
}