using System;
using System.Linq;

using FluentAssertions;

using Highway.Data.Factories;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.ReadonlyTests
{
    [TestClass]
    public class ReadonlyDomainRepositoryTests
    {
        private static IReadonlyRepository _target;

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

            _target = domainRepositoryFactory.CreateReadonly(typeof(SchoolDomain)) as IReadonlyDomainRepository<SchoolDomain>;
        }

        [TestMethod]
        public void CanExecuteQuery()
        {
            var students = _target.Find(new GetStudents()).ToList();
            students.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void CanExecuteScalar()
        {
            var bill = _target.Find(new GetStudentByName("Bill"));
            bill.Grade.Name.Should().Be("first");
        }
    }
}
