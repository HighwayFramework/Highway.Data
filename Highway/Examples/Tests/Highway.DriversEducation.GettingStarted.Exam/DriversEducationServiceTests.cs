
using System.Linq;
using FluentAssertions;
using Highway.Data;
using Highway.Data.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Highway.DriversEducation.GettingStarted.Exam
{
    [TestClass]
    public class DriversEducationServiceTests
    {
        [TestMethod]
        public void ShouldAddDriverByName()
        {
            //arrange
            var context = new InMemoryDataContext();
            var service = new DriversEducationService(new Repository(context));

            //act
            service.AddDriver("Devlin");
            context.Commit();

            //assert
            context.AsQueryable<Driver>().Count(x => x.LastName == "Devlin").Should().Be(1);
        }

        [TestMethod]
        public void ShouldQueryDriversByName()
        {
            //arrange 
            var context = new InMemoryDataContext();
            context.Add(new Driver("Devlin", "Liles"));
            context.Add(new Driver("Tim", "Rayburn"));
            context.Commit();

            var service = new DriversEducationService(new Repository(context));

            //act
            Driver driver = service.GetDriver("Liles");

            //assert
            driver.Should().NotBeNull();
        }
    }
}