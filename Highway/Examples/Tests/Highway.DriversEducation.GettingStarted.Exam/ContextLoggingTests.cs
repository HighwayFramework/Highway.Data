
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data;
using Highway.DriversEducation.GettingStarted.Exam.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Highway.DriversEducation.GettingStarted.Exam
{
    [TestClass]
    public class ContextLoggingTests
    {
        [TestMethod]
        public void ShouldLogAtDebugLevel()
        {
            //arrange 
            var logger = new ConsoleOutLogger("Testing", LogLevel.Trace, true, true, true, string.Empty);
            var target = new DataContext(Settings.Default.Connection, new DriversEducationMappings(), logger);

            //act
            var firstDriver = new Driver("Devlin", "Liles");
            target.Add(firstDriver);
            target.Add(new Driver("Tim", "Rayburn"));
            target.Add(new Driver("Jay", "Smith"));
            target.Add(new Driver("Brian", "Sullivan"));
            target.Add(new Driver("Cori", "Drew"));

            target.Commit();

            target.Reload(firstDriver);

            foreach (var driver in target.AsQueryable<Driver>())
            {
                target.Remove(driver);
            }


            target.Commit();

            target.ExecuteSqlQuery<Driver>("Select * from Drivers Where LastName = @lastName",
                new DbParameter[] {new SqlParameter("lastName", "Liles")});

            //assert
            //Assert.Inconclusive("We fail here to get the output from console nice and easy");
        }
    }

    public class DriversEducationMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>();
        }
    }
}