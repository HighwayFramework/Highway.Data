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
        [TestMethod]
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
    }
}
