using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.Utilities;

namespace Highway.Data.Tests.Utility.CloneExtension
{
    public class Employer
    {
        public Employee Ceo { get; set; }
    }

    public class Employee : Person
    {
    }

    public class Person
    {
        public string FirstName { get; set; }
    }

    [TestClass]
    public class CloneExtensionComplextTypesTests
    {
        [TestMethod]
        public void ShouldCloneReference()
        {
            var employer = new Employer { Ceo = new Employee() };

            var employerClone = employer.Clone();

            Assert.IsNotNull(employerClone.Ceo);
            Assert.AreNotSame(employer.Ceo, employerClone.Ceo);

            //TODO: 
        }

        [TestMethod]
        public void ShouldCopyParentField()
        {
            var employee = new Employee { FirstName = "Clark" };

            var employeeClone = employee.Clone();

            Assert.AreEqual(employee.FirstName, employeeClone.FirstName);
        }
    }
}
