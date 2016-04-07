using System.Data.SqlClient;
using System.Data.Entity;
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
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using NUnit.Framework;

namespace Highway.Data.EntityFramework.Tests.IntegrationTests
{
    [TestFixture]
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
        public override void BeforeAllTests()
        {
            base.BeforeAllTests();
            var initializeDbString = @"
            DECLARE @dbname nvarchar(128)
            SET @dbname = N'Highway.Test'

            IF (EXISTS (SELECT name
              FROM master.dbo.sysdatabases
              WHERE ('[' + name + ']' = @dbname
              OR name = @dbname)))
            BEGIN
              ALTER DATABASE [Highway.Test] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE
            END

            USE master
            DROP DATABASE [Highway.Test]
            CREATE DATABASE [Highway.Test]

            USE [Highway.Test]

            CREATE TABLE Foos (
              [Id]		INT				NOT NULL IDENTITY(1,1) PRIMARY KEY,
              [Name]		NVARCHAR(50)	NULL,
              [Address]	NVARCHAR(50)	NULL,
              [Bar_Id]	INT				NULL
            )

            CREATE TABLE Bars (
              [Id]		INT				NOT NULL IDENTITY(1,1) PRIMARY KEY,
              [Name]		NVARCHAR(50)	NULL,
              [Foo_Id]	INT				NULL,
              [Qux_Id]	INT				NULL
            )

            CREATE TABLE Quxs (
              [Id]		INT				NOT NULL IDENTITY(1,1) PRIMARY KEY,
              [Name]		NVARCHAR(50)	NULL,
              [Bar_Id]	INT				NULL,
              [Qux_Id]	INT				NULL
            )

            CREATE TABLE Bazs (
              [Id]		INT				NOT NULL IDENTITY(1,1) PRIMARY KEY,
              [Name]		NVARCHAR(50)	NULL
            )
            ";
            using (var conn = new SqlConnection(Settings.Default.Connection))
            {
              System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(initializeDbString, conn);
              conn.Open();
              command.ExecuteNonQuery();
            }
        }
        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            Database.SetInitializer(new EntityFrameworkIntializer());
        }

        [Test, Category(TestCategories.Database)]
        public void When_AsQueryable_Called_A_Set_Is_Pulled_From_The_Database()
        {
            //Arrange

            //Act
            IQueryable<Foo> items = target.AsQueryable<Foo>();

            //Assert
            items.Count().Should().BeGreaterOrEqualTo(0);
        }

        [Test, Category(TestCategories.Database)]
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

        [Test, Category(TestCategories.Database)]
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

        [Test, Category(TestCategories.Database)]
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
