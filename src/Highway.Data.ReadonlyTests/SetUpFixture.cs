using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

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
            var connectionString = TestConfiguration.Instance.TestDatabaseConnectionString;
            using (var ctx = new SchoolContext(connectionString))
            {
                var stud = new Student
                    { StudentName = "Bill" };

                ctx.Students.Add(stud);
                ctx.SaveChanges();
            }
        }
    }

    public class Student
    {
        public DateTime? DateOfBirth { get; set; }

        public Grade Grade { get; set; }

        public decimal Height { get; set; }

        public byte[] Photo { get; set; }

        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public float Weight { get; set; }
    }

    public class Grade
    {
        public int GradeId { get; set; }

        public string GradeName { get; set; }

        public string Section { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    public class SchoolContext : DbContext
    {
        public SchoolContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Student> Students { get; set; }
    }
}
