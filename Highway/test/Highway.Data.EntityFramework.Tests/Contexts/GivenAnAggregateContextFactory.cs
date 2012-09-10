using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Castle.Windsor;
using Common.Logging;
using Highway.Data.EntityFramework.Tests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.Tests.TestDomain;
using Highway.Data.EntityFramework;
using Highway.Data.Interfaces;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.WindsorAdapter;
using Castle.MicroKernel.Registration;
using Common.Logging.Simple;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class GivenAnAggregateContextFactory
    {
        //INFO: Deploy the test database before running this
        [TestMethod, TestCategory("Integration"), TestCategory("Database")]
        public void ShouldAddToTheCorrectContext()
        {
            // Arrange
            var container = new WindsorContainer();
            var configuration = new AggregateConfiguration("Test",new IMappingConfiguration[]{new FooMappingConfiguration(),new BarMappingConfiguration()}, null,null ,new[]{typeof(Foo),typeof(Bar)} );
            container.Register(
                Component.For<IAggregateConfiguration>().Instance(configuration)
                .Named(string.Format("{0},{1}",typeof (Foo).FullName,typeof (Bar).FullName)));
            var typeName = string.Format("{0},{1}",typeof(Foo).FullName,typeof(Bar).FullName);
            var windsorServiceLocator = new WindsorServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);

            IDataContext context = AggregateContextFactory.Create<Foo, Bar>();
            var foo = new Foo();
            var bar = new Bar();

            // Act
            context.Add(foo);
            context.Add(bar);

            // Assert
            // peek under the covers and ensure that each add went
            // to the right DbContext.
            var objectContext = ((IObjectContextAdapter) context).ObjectContext;
            Assert.IsTrue(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Count() == 2);
            Assert.IsTrue(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).First().EntitySet.Name == "Foos");
            Assert.IsTrue(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Last().EntitySet.Name == "Bars");
        }

        [TestMethod, TestCategory("Integration"), TestCategory("Database")]
        public void ShouldAllowTwoDifferentContextsToBeLoadedInTheSameAppDomain()
        {
            //Arrange
            var container = new WindsorContainer();
            var configuration = new AggregateConfiguration("Test", new IMappingConfiguration[] { new FooMappingConfiguration(), new BarMappingConfiguration() }, null, null, new[] { typeof(Foo), typeof(Bar) });
            var secondConfiguration = new AggregateConfiguration("Test", new IMappingConfiguration[] { new FooMappingConfiguration() }, null, null, new[] { typeof(Foo) });

            var typeName = string.Format("{0},{1}", typeof(Foo).FullName, typeof(Bar).FullName);
            var secondTypeName = typeof(Foo).FullName;
            container.Register(
                Component.For<IAggregateConfiguration>().Instance(configuration).Named(typeName),
                Component.For<IAggregateConfiguration>().Instance(secondConfiguration).Named(secondTypeName));
            
            var windsorServiceLocator = new WindsorServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);

            IDataContext context = AggregateContextFactory.Create<Foo, Bar>();
            IDataContext contextTwo = AggregateContextFactory.Create<Foo>();
            var foo = new Foo();
            var bar = new Bar();

            // Act
            context.Add(foo);
            context.Add(bar);

            contextTwo.Add(foo);

            // Assert
            // peek under the covers and ensure that each add went
            // to the right DbContext.
            Assert.AreNotSame(context.GetType().FullName, contextTwo.GetType().FullName);
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            Assert.IsTrue(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Count() == 2);
            Assert.IsTrue(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).First().EntitySet.Name == "Foos");
            Assert.IsTrue(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Last().EntitySet.Name == "Bars");

            var objectContextTwo = ((IObjectContextAdapter)contextTwo).ObjectContext;
            Assert.IsTrue(objectContextTwo.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Count() == 1);
            Assert.IsTrue(objectContextTwo.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Single().EntitySet.Name == "Foos");

        }
    }
}
