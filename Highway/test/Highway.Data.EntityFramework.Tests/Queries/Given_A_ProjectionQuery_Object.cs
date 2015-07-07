﻿using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using CommonServiceLocator.WindsorAdapter;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.TestQueries;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.Queries
{
    [TestClass]
    public class Given_A_ProjectionQuery_Object
    {
        internal static IWindsorContainer container;

        [ClassInitialize]
        public static void SetupClass(TestContext context)
        {
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Register(Component.For<IDataContext>().ImplementedBy<TestDataContext>().DependsOn(
                new {connectionString = Settings.Default.Connection}).LifestyleTransient(),
                Component.For<IMappingConfiguration>().ImplementedBy<FooMappingConfiguration>().
                    LifestyleTransient(),
                Component.For<ILog>().ImplementedBy<NoOpLogger>());
        }

        [TestMethod]
        public void When_Passing_To_A_Repository_Query_Object_Then_It_Executes_Against_Context()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var repository = new Repository(context);

            //Act
            repository.Find(new FindFooName());

            //Assert
            context.VerifyAllExpectations();
        }

        [TestMethod]
        public void When_Executed_Returns_An_IEnumerable_Of_Items()
        {
            //Arrange
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>().AsQueryable()).Repeat.Once();
            var query = new FindFooName();


            //Act
            IEnumerable<string> items = query.Execute(context);

            //Assert
            context.VerifyAllExpectations();
            items.Should().NotBeNull();
        }

        [TestMethod]
        public void When_Paging_Should_Affect_The_Base_Query_Before_It_Is_Executed()
        {
            //Arrange
            var targetFoo = new Foo {Name = "Test"};
            var context = MockRepository.GenerateStrictMock<IDataContext>();
            context.Expect(x => x.AsQueryable<Foo>()).Return(new List<Foo>
            {
                new Foo(),
                new Foo(),
                new Foo(),
                new Foo(),
                targetFoo
            }.AsQueryable()).Repeat.Once();
            var query = new FindFooName();

            //Act
            IEnumerable<string> retVal = query.Skip(4).Take(1).Execute(context);


            //Assert
            retVal.First().Should().Be(targetFoo.Name);
        }

        [TestMethod]
        public void When_Calling_Output_Sql_with_Context_It_Outputs_SQL()
        {
            //arrange
            var target = new FindFooName();

            var context = container.Resolve<IDataContext>();

            //act
            string sqlOutput = target.OutputQuery(context);

            //assert

            sqlOutput.Should().NotBeNull();
            sqlOutput.Should().Contain("FROM");
        }
    }
}