using System;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Factory;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Highway.Data.Tests;
using Highway.Data.EntityFramework.Tests.Initializer;
using Castle.MicroKernel.Registration;
using Highway.Data.Interfaces;
using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EventManagement;
using Highway.Data.EntityFramework.Repositories;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class AClassThatShouldNotExist : Context
    {
        public AClassThatShouldNotExist(string connectionString, IMappingConfiguration[] configurations)
            : base(connectionString, configurations)
        {
        }
    }

    [TestClass]
    public class Given_A_Generic_Repository_Factory_With_Many_Types : ContainerTest<RepositoryFactory>
    {
        private BaseMappingConfiguration fooMapping;
        private BaseMappingConfiguration barMapping;
        private BaseMappingConfiguration bazMapping;
        private BaseMappingConfiguration quxMapping;
        private string connString;

        public override RepositoryFactory ResolveTarget()
        {
            return Container.Resolve<RepositoryFactory>(new
            {
                connectionString = connString
            });
        }

        public override void RegisterComponents(Castle.Windsor.IWindsorContainer container)
        {
            fooMapping = new FooMappingConfiguration();
            barMapping = new BarMappingConfiguration();
            bazMapping = new BazMappingConfiguration();
            quxMapping = new QuxMappingConfiguration();
            connString = Settings.Default.Connection;

            container.Register(
                Component.For<IMappingConfiguration>().Instance(fooMapping).Named("Foo"),
                Component.For<IMappingConfiguration>().Instance(barMapping).Named("Bar"),
                Component.For<IMappingConfiguration>().Instance(bazMapping).Named("Baz"),
                Component.For<IMappingConfiguration>().Instance(quxMapping).Named("Qux"),
                Component.For<Func<Type, IMappingConfiguration>>()
                    .Instance(x => container.Resolve<IMappingConfiguration>(x.Name)),
                Component.For<IDataContext>().ImplementedBy<AClassThatShouldNotExist>()
                    .DependsOn(new { connectionString = connString }),
                Component.For<IEventManager>().ImplementedBy<EventManager>(),
                Component.For<IRepository>().ImplementedBy<Repository>()
                );
            base.RegisterComponents(container);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            Database.SetInitializer(new ForceDeleteInitializer(new EntityFrameworkIntializer()));
            // We need to ensure the model is built with all possible classes first.
            // So we retrieve something in setup to ensure that is the case.
            // This matches the real world, where Factory is being used to subset the model.
            var junk = Container.Resolve<IDataContext>().AsQueryable<Foo>().ToList();
        }


        [TestMethod]
        public void Should_Get_Mappings_Specific_To_The_Type_Requested_When_Multiple_Types_Are_Requested()
        {
            //Arrange

            //Act
            var repository = target.Create<Foo, Bar, Baz, Qux>();
            repository.Context.AsQueryable<Qux>().ToList();

            //Assert
            Assert.IsTrue(fooMapping.Configured);
            Assert.IsTrue(bazMapping.Configured);
            Assert.IsTrue(barMapping.Configured);
            Assert.IsTrue(quxMapping.Configured);
            using (repository.Context) { }
        }

        [TestMethod]
        public void Should_Get_Mappings_Specific_To_The_Type_Requested()
        {
            //Arrange

            //Act
            var repository = target.Create<Foo>();
            repository.Context.AsQueryable<Foo>().ToList();

            //Assert
            Assert.IsTrue(fooMapping.Configured);
            using (repository.Context) { }
        }

    }
}