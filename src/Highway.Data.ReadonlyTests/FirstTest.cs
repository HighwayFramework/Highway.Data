using System;

using FluentAssertions;

using Highway.Data.ReadonlyTests.SchoolDomain;
using Highway.Data.Repositories;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.ReadonlyTests
{
    [TestClass]
    public class FirstTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var schoolDomain = new SchoolDomain.SchoolDomain();
            var domainContext = new DomainContext<SchoolDomain.SchoolDomain>(schoolDomain);
            var domainRepository = new DomainRepository<SchoolDomain.SchoolDomain>(domainContext, schoolDomain);

            var firstGrade = new Grade
            {
                Name = "first",
                Section = "section one"
            };

            var bill = new Student
            {
                DoB = DateTime.Now.Subtract(TimeSpan.FromDays(365)),
                Grade = firstGrade,
                Height = 60,
                Weight = 180,
                Name = "Bill"
            };

            domainRepository.DomainContext.Add(bill);
            domainRepository.DomainContext.Commit();
        }

        [TestMethod]
        public void TestOne()
        {
            var schoolDomain = new SchoolDomain.SchoolDomain();
            var domainContext = new ReadonlyDomainContext<SchoolDomain.SchoolDomain>(schoolDomain);
            var domainRepository = new ReadonlyDomainRepository<SchoolDomain.SchoolDomain>(domainContext, schoolDomain);
            var bill = domainRepository.Find(new GetStudentByName("Bill"));
            bill.Grade.Name.Should().Be("first");
        }
    }
}
