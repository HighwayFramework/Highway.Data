using System;
using System.Collections.Generic;
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
            _context = new InMemoryDataContext();
            var repo = new Repository(_context);
            var businessEntity = new BusinessEntity(
                1,
                "name",
                "abbr",
                string.Empty,
                new StatusType(),
                new List<EntityType> {new EntityType()},
                new DateTime(2014, 1, 1),
                null);
            repo.Context.Add(businessEntity);
        }


        [TestMethod]
        public void ShouldNotThrowErrorsOnAddWithSimilarThings()
        {
            var repo = new Repository(new InMemoryDataContext());
            var specification = new CreatePollingDeviceSpecification { DeviceModel = "Test" };

            var deviceModel = new DeviceModel
            {
                Id = 1,
                Code = specification.DeviceModel,
                Name = specification.DeviceModel
            };
            repo.Context.Add(deviceModel);
            repo.Context.Commit();
        }
    }

    public class DeviceModel : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class CreatePollingDeviceSpecification
    {
        public string DeviceModel { get; set; }
    }
    

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

        private static void ValidateInitialValues(string abbreviation)
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
            customer.SetEntityType(new EntityType { Id = 1, EntityTypeName = "Customer" });

            Customer = customer;
        }

    }

    public class Customer
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

    public class EntityType
    {
        public int Id { get; set; }

        public string EntityTypeName { get; set; }
    }

    public class StatusType
    {
    }
}