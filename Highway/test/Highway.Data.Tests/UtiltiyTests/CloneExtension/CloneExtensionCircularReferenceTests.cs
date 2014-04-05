using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highway.Data.Utilities;

namespace Highway.Data.Tests.Utility.CloneExtension
{
    [TestClass]
    public class CloneExtensionCircularReferenceSimpleTests
    {
        private Parent _parent;
        private Child _child;

        [TestInitialize]
        public void SetUp()
        {
            this._parent = new Parent();
            this._child = new Child();
            this._parent.Child = this._child;
            this._child.Parent = this._parent;
        }

        [TestMethod]
        public void ShouldGetCloneParentFromChild()
        {
            var parentClone = _parent.Clone();

            Assert.AreNotSame(_child, parentClone.Child);
            Assert.AreNotSame(_parent, parentClone);
            Assert.AreSame(parentClone, parentClone.Child.Parent);
        }

        [TestMethod]
        public void ShouldGetCloneChildFromParent()
        {
            var childClone = _child.Clone();

            Assert.AreNotSame(_child, childClone);
            Assert.AreNotSame(_parent, childClone.Parent);
            Assert.AreSame(childClone, childClone.Parent.Child);
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
    
    [TestClass]
    public class CloneExtensionCircularReferenceComplexTests
    {
        private GrandParent _grandParent;
        private Parent _parent;
        private Child _child;

        [TestInitialize]
        public void SetUp()
        {
            this._grandParent = new GrandParent();
            this._parent = new Parent();
            this._child = new Child();

            this._grandParent.Child = this._parent;
            this._grandParent.GrandChild = this._child;

            this._parent.Child = this._child;
            this._parent.DirectParent = this._grandParent;

            this._child.Parent = this._parent;
            this._child.GrandParent = this._grandParent;
        }

        [TestMethod]
        public void ShouldCloneChild()
        {
            var childClone = _child.Clone();

            AssertEntities(childClone.GrandParent, childClone.Parent, childClone);
        }

        [TestMethod]
        public void ShouldCloneParent()
        {
            var parentClone = _parent.Clone();

            AssertEntities(parentClone.DirectParent, parentClone, parentClone.Child);
        }

        [TestMethod]
        public void ShouldCloneGrandParent()
        {
            var grandParentClone = _grandParent.Clone();

            AssertEntities(grandParentClone, grandParentClone.Child, grandParentClone.GrandChild);
        }

        private void AssertEntities(GrandParent grandParent, Parent parent, Child child)
        {
            Assert.IsNotNull(grandParent);
            Assert.IsNotNull(parent);
            Assert.IsNotNull(child);

            Assert.AreNotSame(this._grandParent, grandParent);
            Assert.AreNotSame(this._parent, parent);
            Assert.AreNotSame(this._child, child);

            Assert.AreSame(grandParent.Child, parent);
            Assert.AreSame(grandParent.GrandChild, child);

            Assert.AreSame(parent.DirectParent, grandParent);
            Assert.AreSame(parent.Child, child);

            Assert.AreSame(child.Parent, parent);
            Assert.AreSame(child.GrandParent, grandParent);
        }

        class Child
        {
            public Parent Parent { get; set; }
            public GrandParent GrandParent { get; set; }
        }

        class Parent
        {
            public Child Child { get; set; }
            public GrandParent DirectParent { get; set; }
        }

        class GrandParent
        {
            public Parent Child { get; set; }
            public Child GrandChild { get; set; }
        }
    }
}
