using Common.Logging.Simple;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.Factories;
using NUnit.Framework;
using Rhino.Mocks;


namespace Highway.Data.EntityFramework.Tests.Factories
{
    [TestFixture]
    public class RepositoryFactoryTests
    {
        [Test]
        public void ShouldCreateRepository()
        {
            // arrange
            IDomainRepositoryFactory factory = new DomainRepositoryFactory(new []{new TestDomain()});

            // act
            IRepository repo = factory.Create<TestDomain>();

            // assert
            repo.Should().NotBeNull();
        }

        [Test]
        public void ShouldCreateRepositoryWhenDomainEventsAreNull()
        {
            // arrange
            var testDomain = new TestDomain { Events = null };
            IDomainRepositoryFactory factory = new DomainRepositoryFactory(new []{testDomain});

            // act
            IRepository repo = factory.Create<TestDomain>();

            // assert
            repo.Should().NotBeNull();
        }

        [Test]
        public void ShouldCreateRepositoryFromType()
        {
            // arrange
            IDomainRepositoryFactory factory = new DomainRepositoryFactory(new[] { new TestDomain() });

            // act
            IRepository repo = factory.Create(typeof (TestDomain));

            // assert
            repo.Should().NotBeNull();
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
