using System;
using System.Collections.Generic;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class TestThrowsExceptionWithListEntityType
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
                new List<EntityType> { new EntityType() },
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
}
