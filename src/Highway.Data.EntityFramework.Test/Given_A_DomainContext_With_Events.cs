using FluentAssertions;
using Highway.Data.EntityFramework.Test.SqlLiteDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Highway.Data.EntityFramework.Test
{
    //[TestClass]
    public class Given_A_DomainContext_With_Events
    {
        private TestDomainContext<SqlLiteDomain.SqlLiteDomain> _context;
        private bool _beforeSaveCalled;
        private bool _afterSaveCalled;

        [TestInitialize]
        public void Setup()
        {
            this._beforeSaveCalled = false;
            this._afterSaveCalled = false;
            _context = new TestDomainContext<SqlLiteDomain.SqlLiteDomain>(new SqlLiteDomain.SqlLiteDomain());
            _context.BeforeSave += (sender, args) => this._beforeSaveCalled = true;
            _context.AfterSave += (sender, args) => this._afterSaveCalled = true;
        }

        [TestMethod]
        public void Should_Call_BeforeSave()
        {
            // Arrange
            _context.Add(new Person { Id = Guid.NewGuid(), FirstName = "Tim", LastName = "Rayburn" });

            // Act
            _context.Commit();

            // Assert
            _beforeSaveCalled.Should().BeTrue();
        }
    }
}
