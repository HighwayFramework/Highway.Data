using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class Given_An_AggregateContextFactory
    {
        [TestMethod]
        public void Should_Add_To_The_Correct_Context()
        {
            // Arrange
            var factory = new AggregateContextFactory();
            var context = factory.Create<Foo, Bar>();
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
