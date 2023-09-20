using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

using FluentAssertions;

using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Repositories;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.ReadonlyTests
{
    [TestClass]
    public class WhenADomainRepositoryWithQueryInterceptorsRunsAQueryWithInterceptors
    {
        private const string ConnectionString = "Data Source=(localDb);Initial Catalog=Highway.Data.Test.Db;Integrated Security=True;Connection Timeout=1";

        private readonly TypeInspectionAfterQueryInterceptor _afterQueryInterceptor = new TypeInspectionAfterQueryInterceptor();

        private readonly TypeInspectionBeforeQueryInterceptor _beforeQueryInterceptor = new TypeInspectionBeforeQueryInterceptor();

        private IEnumerable<Student> _results;

        private GetStudentsWithEvents _testQuery;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            // The following code uses a writeable domain context to insert test data.
            var domain = new SchoolDomain();
            var domainContext = new DomainContext<SchoolDomain>(domain);

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

            domainContext.Add(bill);
            domainContext.Commit();

            // Act
            // The following code uses a readonly domain context to query the test data.
            var readonlyDomain = new SchoolDomain(new List<IInterceptor> { _afterQueryInterceptor, _beforeQueryInterceptor });
            var readonlyDomainContext = new ReadonlyDomainContext<SchoolDomain>(readonlyDomain);
            var readonlyRepository = new ReadonlyDomainRepository<SchoolDomain>(readonlyDomainContext, readonlyDomain);
            _testQuery = new GetStudentsWithEvents();
            _results = readonlyRepository.Find(_testQuery);
        }

        [TestMethod]
        public void The_Query_After_Query_Interceptor_Should_Fire()
        {
            // FAILS
            _testQuery.AfterQueryFired.Should().BeTrue();
        }

        [TestMethod]
        public void The_Query_Before_Query_Interceptor_Should_Fire()
        {
            // FAILS
            _testQuery.BeforeQueryFired.Should().BeTrue();
        }

        [TestMethod]
        public void The_Repository_After_Query_Interceptor_Should_Fire()
        {
            // PASSES
            _afterQueryInterceptor.InspectedType.Should().Be(typeof(DbQuery<Student>));
        }

        [TestMethod]
        public void The_Repository_Before_Query_Interceptor_Should_Fire()
        {
            // PASSES
            _beforeQueryInterceptor.InspectedType.Should().Be(typeof(GetStudentsWithEvents));
        }

        [TestMethod]
        public void The_Results_Should_Be_Populated()
        {
            // PASSES
            _results.Should().HaveCountGreaterThan(0);
        }
    }
}
