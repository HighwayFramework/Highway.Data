using System.Linq;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class TestCircularReference
    {
        private IDataContext _context;
        private Parent _parent;
        private Child _child;

        [TestInitialize]
        public void SetUp()
        {
            _context = new InMemoryDataContext();

            _parent = new Parent();
            _child = new Child();
            _parent.Child = _child;
            _child.Parent = _parent;

            _context.Add(_parent);
            _context.Commit();
        }

        [TestMethod]
        public void ShouldGetSingleChildWithParent()
        {
            var child = _context.AsQueryable<Child>().Single();

            Assert.AreEqual(_child, child);
            Assert.AreEqual(_parent, child.Parent);
        }

        [TestMethod]
        public void ShouldGetSingleParentWithChild()
        {
            var parent = _context.AsQueryable<Parent>().Single();

            Assert.AreEqual(_parent, parent);
            Assert.AreEqual(_child, parent.Child);
        }

        class Child
        {
            public Parent Parent { get; set; }
        }

        class Parent
        {
            public Child Child { get; set; }
        }
    }
}