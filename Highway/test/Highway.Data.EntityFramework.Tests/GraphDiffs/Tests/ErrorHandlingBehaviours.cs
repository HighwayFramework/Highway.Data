using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactorThis.GraphDiff;

namespace Highway.Data.EntityFramework.Tests.GraphDiffs.Tests
{
    [TestClass]
    public class ErrorHandlingBehaviours : TestBase
    {
        internal class UnknownType { }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowIfTypeIsNotKnown()
        {
            using (var context = new TestDbContext())
            {
                context.UpdateGraph(new UnknownType());
                context.SaveChanges();
            }
        }
    }
}
