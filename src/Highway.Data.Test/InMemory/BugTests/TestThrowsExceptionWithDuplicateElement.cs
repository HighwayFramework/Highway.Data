using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class TestThrowsExceptionWithDuplicateElement
    {
        private IDataContext _context;

        [TestMethod]
        public void ShouldNotThrowErrorOnCommit()
        {
            _context = new InMemoryDataContext();
            var entityType = new EntityType { Id = 1, EntityTypeName = "Customer" };
            _context.Add(entityType);
            _context.Commit();

            var businessEntity = new BusinessEntity(
                new List<EntityType>
                    { entityType });

            _context.Add(businessEntity);
            _context.Commit();
        }

        [TestMethod]
        public void ShouldRetrieveWithChildProperty()
        {
            _context = new InMemoryDataContext();
            var entityType = new EntityType { Id = 1, EntityTypeName = "Customer" };
            _context.Add(entityType);
            _context.Commit();

            var businessEntity = new BusinessEntity(
                new List<EntityType>
                    { entityType });

            _context.Add(businessEntity);
            _context.Commit();

            entityType.Should().Be(_context.AsQueryable<BusinessEntity>().Single().EntityTypes.First());
        }

        private class BusinessEntity : IIdentifiable<long>
        {
            public BusinessEntity(List<EntityType> entityTypes)
            {
                EntityTypes = entityTypes;
            }

            public List<EntityType> EntityTypes { get; }

            public long Id { get; set; }
        }

        private class EntityType
        {
            public string EntityTypeName { get; set; }

            public int Id { get; set; }
        }
    }
}
