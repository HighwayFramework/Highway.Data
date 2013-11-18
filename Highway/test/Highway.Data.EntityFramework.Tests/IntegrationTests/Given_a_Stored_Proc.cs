#region

using System.Data.Entity;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.EntityFramework.Tests.Initializer;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.TestQueries;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.EntityFramework.Tests.IntegrationTests
{
    [TestClass]
    public class Given_a_Stored_Proc : ContainerTest<TestDataContext>
    {
        public override TestDataContext ResolveTarget()
        {
            return Container.Resolve<TestDataContext>(new {connectionString = Settings.Default.Connection});
        }

        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<IMappingConfiguration>().ImplementedBy<FooMappingConfiguration>(),
                Component.For<ILog>().ImplementedBy<NoOpLogger>(),
                Component.For<IContextConfiguration>().ImplementedBy(null));

            base.RegisterComponents(container);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            Database.SetInitializer(new EntityFrameworkIntializer());
            target.AsQueryable<Foo>().ToList();
            target.Add(new Foo {Name = "Devlin"});
            target.Add(new Foo {Name = "Tim"});
            target.SaveChanges();
        }

        [TestMethod, TestCategory(TestCategories.Database)]
        public void Should_Call_Stored_Procedure_and_Return_An_Entity()
        {
            //Arrange
            var repository = new Repository(target);

            //Act
            var results = repository.Find(new StoredProcTestQuery());

            //Assert
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.All(x => x.Name == "Devlin"));
        }
    }
}