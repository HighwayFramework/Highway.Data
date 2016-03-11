using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data;
using Highway.Data.InMemory;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory.BugTests
{
    [TestFixture]
    public class InMemDuplicateElementBugTests
    {
        private IDataContext _context;

        [Test]
        public void ShouldNotThrowErrorOnCommit()
        {
            this._context = new InMemoryDataContext();
            var entityType = new EntityType { Id = 1, EntityTypeName = "Customer" };
            this._context.Add(entityType);
            this._context.Commit();

            var businessEntity = new BusinessEntity(
                new List<EntityType>() { entityType });
            this._context.Add(businessEntity);
            this._context.Commit();
        }

        [Test]
        public void ShouldRetrieveWithChildProperty()
        {
            this._context = new InMemoryDataContext();
            var entityType = new EntityType { Id = 1, EntityTypeName = "Customer" };
            this._context.Add(entityType);
            this._context.Commit();

            var businessEntity = new BusinessEntity(
                new List<EntityType>() { entityType });
            this._context.Add(businessEntity);
            this._context.Commit();

            Assert.AreEqual(entityType, this._context.AsQueryable<BusinessEntity>().Single().EntityTypes.First());
        }

        class BusinessEntity : IIdentifiable<long>
        {

            public BusinessEntity(List<EntityType> entityTypes)
            {
                EntityTypes = entityTypes;
            }

            public long Id { get; set; }

            public List<EntityType> EntityTypes { get; protected set; }

        }

        class EntityType
        {
            public int Id { get; set; }

            public string EntityTypeName { get; set; }
        }
    }
}
