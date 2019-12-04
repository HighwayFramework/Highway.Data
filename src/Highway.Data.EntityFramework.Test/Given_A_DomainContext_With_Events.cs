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
        private TestDomainContext<SqlLiteDomain.SqlLiteDomain> context;
        private bool BeforeSaveCalled;
        private bool AfterSaveCalled;

        [TestInitialize]
        public void Setup()
        {
            this.BeforeSaveCalled = false;
            this.AfterSaveCalled = false;
            context = new TestDomainContext<SqlLiteDomain.SqlLiteDomain>(new SqlLiteDomain.SqlLiteDomain());
            context.BeforeSave += (sender, args) => this.BeforeSaveCalled = true;
            context.AfterSave += (sender, args) => this.AfterSaveCalled = true;
        }

        [TestMethod]
        public void Should_Call_BeforeSave()
        {
            // Arrange
            context.Add(new Person { Id = Guid.NewGuid(), FirstName = "Tim", LastName = "Rayburn" });

            // Act
            context.Commit();

            // Assert
            BeforeSaveCalled.Should().BeTrue();
        }
    }
}
