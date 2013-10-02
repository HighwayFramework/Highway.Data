using System;
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

            //assert
            context.AsQueryable<Driver>().Count(x => x.Name == "Devlin").Should().Be(1);
        }

        [TestMethod]
        public void ShouldQueryDriversByName()
        {
            //arrange 
            var context = new InMemoryDataContext();
            context.Add(new Driver("Devlin"));
            context.Add(new Driver("Tim"));

            var service = new DriversEducationService(new Repository(context));

            //act
            Driver driver = service.GetDriver("Devlin");

            //assert
            driver.Should().NotBeNull();
        }
    }
}
