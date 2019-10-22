﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.Initializer;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.IntegrationTests
{
    [TestClass]
    public class AssemblyConfigurationTests : ContainerTest<TestDataContext>
    {
     public override TestDataContext ResolveTarget()
        {
            return Container.Resolve<TestDataContext>(new {connectionString = Settings.Default.Connection});
        }

        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<IMappingConfiguration>().Instance(Mappings.FromAssemblyContaing<FooMap>().FromAssemblyContaing<BarMap>()),
                Component.For<ILog>().ImplementedBy<NoOpLogger>(),
                Component.For<IContextConfiguration>().ImplementedBy(null));

            base.RegisterComponents(container);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            Database.SetInitializer(new EntityFrameworkIntializer());
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            IQueryable<Foo> items = target.AsQueryable<Foo>();

            //Assert
            items.Count().Should().BeGreaterOrEqualTo(0);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Add_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_An_Added_State()
        {
            //Arrange
            var item = new Foo();

            //Act
            target.Add(item);

            //Assert
            target.ChangeTracker.DetectChanges();
            DbEntityEntry<Foo> entry = target.Entry(item);
            entry.State.Should().Be(EntityState.Added);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Remove_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_A_Deleted_State()
        {
            //Arrange

            //Act
            Foo item = target.AsQueryable<Foo>().First();
            target.Remove(item);

            //Assert
            target.ChangeTracker.DetectChanges();
            DbEntityEntry<Foo> entry = target.Entry(item);
            entry.State.Should().Be(EntityState.Deleted);
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void When_Detach_Is_Called_The_Object_Is_Added_To_The_ChangeTracker_In_A_Detached_State()
        {
            //Arrange

            //Act
            Foo item = target.AsQueryable<Foo>().First();
            target.Detach(item);

            //Assert
            target.ChangeTracker.DetectChanges();
            DbEntityEntry<Foo> entry = target.Entry(item);
            entry.State.Should().Be(EntityState.Detached);
            target.Dispose();
        }
    }
}