using System;
using Highway.Data.EntityFramework.Tests.GraphDiffs.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactorThis.GraphDiff;

namespace Highway.Data.EntityFramework.Tests.GraphDiffs.Tests
{
    [TestClass]
    public class AttachedBehaviours
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowExceptionIfAggregateIsNotDetached()
        {
            using (var context = new TestDbContext())
            {
                var node = new TestNode();
                context.Nodes.Add(node);
                node.Title = "Hello";
                context.UpdateGraph(node);
                context.SaveChanges();
            }
        }
    }
}
