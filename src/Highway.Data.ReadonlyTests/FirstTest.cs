using System;

using FluentAssertions;

using Highway.Data.Factories;
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
            var schoolDomain = new SchoolDomain();
            var domainRepositoryFactory = new DomainRepositoryFactory(new IDomain[] { schoolDomain });
            var domainRepository = domainRepositoryFactory.Create(typeof(SchoolDomain));

            var firstGrade = new Grade
            {
                Name = "first",
                Section = "section one"
            };

            var bill = new Student
            {
                DoB = DateTime.Now.Subtract(TimeSpan.FromDays(365)),
                Height = 60,
                Weight = 180,
                Name = "Bill"
            };

            firstGrade.AddStudent(bill);

            domainRepository.Context.Add(bill);
            domainRepository.Context.Commit();
        }

        [TestMethod]
        public void TestOne()
        {
            var schoolDomain = new SchoolDomain();
            var domainRepositoryFactory = new DomainRepositoryFactory(new IDomain[] { schoolDomain });
            var domainRepository = domainRepositoryFactory.CreateReadonly(typeof(SchoolDomain));
            var bill = domainRepository.Find(new GetStudentByName("Bill"));
            bill.Grade.Name.Should().Be("first");
        }
    }
}
