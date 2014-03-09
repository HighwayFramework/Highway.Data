using System;
using System.Collections.Generic;

using Highway.Data;
using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class InMemBugTests
    {
        private IDataContext _context;

        [TestMethod]
        public void ShouldNotThrowErrorOnAdd()
        {
            this._context = new InMemoryDataContext();
            var businessEntity = new BusinessEntity(
                1,
                "name",
                "abbr",
                string.Empty,
                new StatusType(),
                new List<EntityType>() { new EntityType() },
                new DateTime(2014, 1, 1),
                null);
            this._context.Add(businessEntity);
       }

        class BusinessEntity : IIdentifiable<long>
        {

            public BusinessEntity(long id, string name, string abbreviation, string epaNumber, StatusType status, List<EntityType> entityTypes, DateTime startDate, DateTime? endDate)
            {
                ValidateInitialValues(name, abbreviation, entityTypes);

                EntityTypes = entityTypes;
                Id = id;
                Name = name;
                Abbreviation = abbreviation;
                EpaNumber = epaNumber;
                EntityStatusType = status;
                StartDate = startDate;
                EndDate = endDate;

            }

            private static void ValidateInitialValues(string name, string abbreviation, List<EntityType> entityTypes)
            {
                if (abbreviation.Length != 4)
                {
                    throw new ArgumentOutOfRangeException(abbreviation, "Abbreviation must be four characters long.");
                }
            }

            public long Id { get; set; }

            public string Name { get; protected set; }

            public string Abbreviation { get; protected set; }

            public string EpaNumber { get; protected set; }

            public List<EntityType> EntityTypes { get; protected set; }

            public StatusType EntityStatusType { get; protected set; }

            public DateTime StartDate { get; protected set; }

            public DateTime? EndDate { get; protected set; }

            public DateTime CreateDate { get; protected set; }

            public string CreatedBy { get; protected set; }

            public DateTime LastModifiedDate { get; protected set; }

            public string LastModifiedBy { get; protected set; }

            public Customer Customer { get; protected set; }

            public void SetCustomer(Customer customer)
            {
                customer.SetBusinessEntity(this);
                customer.SetEntityType(new EntityType() { Id = 1, EntityTypeName = "Customer" });

                this.Customer = customer;
            }

        }

        class Customer
        {
            public void SetBusinessEntity(BusinessEntity businessEntity)
            {
                throw new NotImplementedException();
            }

            public void SetEntityType(EntityType entityType)
            {
                throw new NotImplementedException();
            }
        }

        class EntityType
        {
            public int Id { get; set; }

            public string EntityTypeName { get; set; }
        }

        class StatusType
        {
        }
    }
}