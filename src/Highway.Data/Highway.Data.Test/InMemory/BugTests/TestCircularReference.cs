using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data;
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
            this._context = new InMemoryDataContext();

            this._parent = new Parent();
            this._child = new Child();
            this._parent.Child = this._child;
            this._child.Parent = this._parent;

            this._context.Add(_parent);
            this._context.Commit();
        }

        [TestMethod]
        public void ShouldGetSingleChildWithParent()
        {
            var child = this._context.AsQueryable<Child>().Single();

            Assert.AreEqual(this._child, child);
            Assert.AreEqual(this._parent, child.Parent);
        }

        [TestMethod]
        public void ShouldGetSingleParentWithChild()
        {
            var parent = this._context.AsQueryable<Parent>().Single();

            Assert.AreEqual(this._parent, parent);
            Assert.AreEqual(this._child, parent.Child);
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