using System.Collections.Generic;
using FluentAssertions;
using Highway.Data.Domain;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

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
            context.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldCreateOneOfManyContexts()
        {
            // arrange
            IContextFactory factory = new ContextFactory(new FooDomain(), new BarDomain());

            // act
            IDataContext context = factory.Create<FooDomain>();

            // assert
            context.Should().BeAssignableTo<IDomainContext<FooDomain>>();
        }


        [TestMethod]
        public void ShouldCreateOneOfManyContextsBasedOnType()
        {
            // arrange
            IContextFactory factory = new ContextFactory(new FooDomain(), new BarDomain());

            // act
            IDataContext context = factory.Create(typeof (FooDomain));

            // assert
            context.Should().BeAssignableTo<IDataContext>();
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
            context1.Should().BeAssignableTo<IDataContext>();
            context2.Should().BeAssignableTo<IDataContext>();
        }
    }

    public class BarDomain : IDomain
    {
        public BarDomain()
        {
            ConnectionString = Settings.Default.Connection;
            Events = new List<IInterceptor>();
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration Mappings { get; set; }

        public IContextConfiguration Context { get; set; }
        public List<IInterceptor> Events { get; set; }
    }


    public class FooDomain : IDomain
    {
        public FooDomain()
        {
            ConnectionString = Settings.Default.Connection;
            Events = new List<IInterceptor>();
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration Mappings { get; set; }

        public IContextConfiguration Context { get; set; }
        public List<IInterceptor> Events { get; set; }
    }
}