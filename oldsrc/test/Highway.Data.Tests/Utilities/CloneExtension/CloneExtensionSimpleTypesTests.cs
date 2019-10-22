using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.Utilities;

namespace Highway.Data.Tests.Utilities.CloneExtension
{
    [TestClass]
    public class CloneExtensionSimplePublicTests
    {
        public class Car
        {
            private int topSpeed;

            public int TopSpeed
            {
                get { return topSpeed; }
                set { topSpeed = value; }
            }

            public int Doors { get; set; }

            public Car()
            {
            }
        }


        [TestMethod]
        public void ShouldCreateCopyWithPublicDefaultConstructor()
        {
            var car = new Car();

            var carClone = car.Clone();

            Assert.IsNotNull(carClone);
            Assert.IsInstanceOfType(carClone, typeof(Car));
        }

        [TestMethod]
        public void ShouldCopyPrimitiveImplicitPublicProperty()
        {
            var car = new Car();
            car.Doors = 2;

            var carClone = car.Clone();

            Assert.AreEqual(car.Doors, carClone.Doors);
        }

        [TestMethod]
        public void ShouldCopyPrimitiveExplicitPublicProperty()
        {
            var car = new Car();
            car.TopSpeed = 200;

            var carClone = car.Clone();

            Assert.AreEqual(car.TopSpeed, carClone.TopSpeed);
        }
    }

    [TestClass]
    public class CloneExtensionSimplePrivateTests
    {
        [TestMethod]
        public void ShouldCreateCopyWithPrivateDefaultConstructor()
        {
            var person = new Person("Clark");

            var humanClone = person.Clone();

            Assert.IsNotNull(humanClone);
            Assert.IsInstanceOfType(humanClone, typeof(Person));
        }

        [TestMethod]
        public void ShouldCopyPrimitivePropertyWithPrivateSetter()
        {
            var person = new Person("Clark");

            var humanClone = person.Clone();

            Assert.AreEqual(person.FirstName, humanClone.FirstName);
        }

        [TestMethod]
        public void ShouldCopyPrivateFields()
        {
            var person = new Person("Clark", true);

            var humanClone = person.Clone();

            Assert.AreEqual(person.IsIntrovert, humanClone.IsIntrovert);
        }

        public class Person
        {
            private Person()
            {

            }

            private bool isIntrovert;

            public string FirstName { get; private set; }

            public Person(string firstName, bool isIntrovert = false)
            {
                FirstName = firstName;
                this.isIntrovert = isIntrovert;
            }

            public bool IsIntrovert { get { return isIntrovert; } }
        }

    }
}
