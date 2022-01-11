using System;
using System.Data.SqlClient;

using FluentAssertions;

using Highway.Data.Repositories;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.ReadonlyTests
{
    [TestClass]
    public class SetUpFixture
    {
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            using (var masterSqlConnection = new SqlConnection(TestConfiguration.Instance.MasterConnectionString))
            {
                masterSqlConnection.Open();
                using (var command = masterSqlConnection.CreateCommand())
                {
                    command.CommandText = $"EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'[{TestConfiguration.Instance.TestDatabaseName}]';"
                        + $"USE [master]; ALTER DATABASE [{TestConfiguration.Instance.TestDatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;"
                        + $"DROP DATABASE [{TestConfiguration.Instance.TestDatabaseName}];";

                    command.ExecuteNonQuery();
                }
            }
        }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Console.WriteLine(context.TestName);

            using (var masterSqlConnection = new SqlConnection(TestConfiguration.Instance.MasterConnectionString))
            {
                masterSqlConnection.Open();
                using (var command = masterSqlConnection.CreateCommand())
                {
                    command.CommandText = $"CREATE DATABASE [{TestConfiguration.Instance.TestDatabaseName}]";

                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void TestOne()
        {
            var schoolDomain = new SchoolDomain();
            var domainContext = new DomainContext<SchoolDomain>(schoolDomain);
            var domainRepository = new DomainRepository<SchoolDomain>(domainContext, schoolDomain);

            var firstGrade = new Grade
            {
                GradeName = "first",
                Section = "section one"
            };

            var bill = new Student
            {
                DateOfBirth = DateTime.Now.Subtract(TimeSpan.FromDays(365)),
                Grade = firstGrade,
                Height = 60,
                Weight = 180,
                StudentName = "Bill"
            };

            domainRepository.DomainContext.Add(bill);
            domainRepository.DomainContext.Commit();

            bill.StudentID.Should().NotBe(0);
        }
    }
}
