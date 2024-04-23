using System;
using System.Collections.Generic;

namespace Highway.Data.Tests.InMemory.BugTests
{
    public class BusinessEntity : IIdentifiable<long>
    {
        public BusinessEntity(long id, string name, string abbreviation, string epaNumber, StatusType status, List<EntityType> entityTypes, DateTime startDate, DateTime? endDate)
        {
            ValidateInitialValues(abbreviation);

            EntityTypes = entityTypes;
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
            EpaNumber = epaNumber;
            EntityStatusType = status;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string Abbreviation { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        public string CreatedBy { get; protected set; }

        public Customer Customer { get; protected set; }

        public DateTime? EndDate { get; protected set; }

        public StatusType EntityStatusType { get; protected set; }

        public List<EntityType> EntityTypes { get; protected set; }

        public string EpaNumber { get; protected set; }

        public long Id { get; set; }

        public string LastModifiedBy { get; protected set; }

        public DateTime LastModifiedDate { get; protected set; }

        public string Name { get; protected set; }

        public DateTime StartDate { get; protected set; }

        public void SetCustomer(Customer customer)
        {
            customer.SetBusinessEntity(this);
            customer.SetEntityType(new EntityType { Id = 1, EntityTypeName = "Customer" });

            Customer = customer;
        }

        private static void ValidateInitialValues(string abbreviation)
        {
            if (abbreviation.Length != 4)
            {
                throw new ArgumentOutOfRangeException(abbreviation, "Abbreviation must be four characters long.");
            }
        }
    }
}