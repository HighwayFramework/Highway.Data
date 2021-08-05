using System;

using FluentAssertions;

using Highway.Data.EntityFramework.Test.SqlLiteDomain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Test
{
    [Ignore("These tests spin forever.")]
    [TestClass]
    public class GivenADomainContextWithEvents
    {
        private bool _afterSaveCalled;

        private bool _beforeSaveCalled;

        private TestDomainContext<SqlLiteDomain.SqlLiteDomain> _context;

        [TestInitialize]
        public void Setup()
        {
            _beforeSaveCalled = false;
            _afterSaveCalled = false;
            _context = new TestDomainContext<SqlLiteDomain.SqlLiteDomain>(new SqlLiteDomain.SqlLiteDomain());
            _context.BeforeSave += (sender, args) => _beforeSaveCalled = true;
            _context.AfterSave += (sender, args) => _afterSaveCalled = true;
        }

        [TestMethod]
        public void Should_Call_AfterSave()
        {
            // Arrange
            _context.Add(new Person { Id = Guid.NewGuid(), FirstName = "Devlin", LastName = "Liles" });

            // Act
            _context.Commit();

            // Assert
            _afterSaveCalled.Should().BeTrue();
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
