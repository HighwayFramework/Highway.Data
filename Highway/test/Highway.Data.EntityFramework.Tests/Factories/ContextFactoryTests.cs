#region

using Highway.Data.Domain;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

#endregion

namespace Highway.Data.EntityFramework.Tests.Factories
{
    [TestClass]
    public class ContextFactoryTests
    {
        [TestMethod]
        public void ShouldCreateContext()
        {
            //arrange 
            IContextFactory factory = new ContextFactory(new FooDomain());

            //act
            IDataContext context = factory.Create<FooDomain>();

            //assert
            context.ShouldNotBeNull();
        }

        [TestMethod]
        public void ShouldCreateOneOfManyContexts()
        {
            // arrange
            IContextFactory factory = new ContextFactory(new FooDomain(), new BarDomain());

            // act
            IDataContext context = factory.Create<FooDomain>();

            // assert
            context.ShouldBeOfType<IDomainContext<FooDomain>>();
        }


        [TestMethod]
        public void ShouldCreateOneOfManyContextsBasedOnType()
        {
            // arrange
            IContextFactory factory = new ContextFactory(new FooDomain(), new BarDomain());

            // act
            IDataContext context = factory.Create(typeof (FooDomain));

            // assert
            context.ShouldBeOfType<IDataContext>();
        }


        [TestMethod]
        public void ShouldCreateManyOfManyContextsBasedOnType()
        {
            // arrange
            var barDomain = new BarDomain();
            var fooDomain = new FooDomain();
            IContextFactory factory = new ContextFactory(fooDomain, barDomain);
            var barMappings = MockRepository.GenerateMock<IMappingConfiguration>();
            var fooMappings = MockRepository.GenerateMock<IMappingConfiguration>();
            barDomain.Mappings = barMappings;
            fooDomain.Mappings = fooMappings;

            // act
            IDataContext context1 = factory.Create(typeof (FooDomain));
            IDataContext context2 = factory.Create(typeof (BarDomain));

            // assert
            context1.ShouldBeOfType<IDataContext>();
            context2.ShouldBeOfType<IDataContext>();
        }
    }

    public class BarDomain : IDomain
    {
        public BarDomain()
        {
            ConnectionString = Settings.Default.Connection;
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration Mappings { get; set; }

        public IContextConfiguration Context { get; set; }
    }


    public class FooDomain : IDomain
    {
        public FooDomain()
        {
            ConnectionString = Settings.Default.Connection;
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration Mappings { get; set; }

        public IContextConfiguration Context { get; set; }
    }
}