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
    public class Given_An_AggregateContextFactory
    {
        [TestMethod]
        public void Should_Add_To_The_Correct_Context()
        {
            // Arrange
            var container = new Castle.Windsor.WindsorContainer();
            var configuration = new AggregateConfiguration("Test",new[]{});
            var typeName = string.Format("{0},{1}",typeof(Foo).FullName,typeof(Bar).FullName);
            container.Register(Component.For<IAggregateConfiguration>().Instance(configuration).Named(typeName));
            var provider = new WindsorServiceLocator(container);

            var factory = new AggregateContextFactory();
            IDataContext context = factory.Create<Foo, Bar>();
            var foo = new Foo();
            var bar = new Bar();

            // Act
            context.Add(foo);
            context.Add(bar);

            // Assert
            // peek under the covers and ensure that each add went
            // to the right DbContext.
        }
    }
}
