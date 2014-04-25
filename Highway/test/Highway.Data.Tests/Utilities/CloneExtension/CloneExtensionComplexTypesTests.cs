using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Highway.Data.Utilities;
using System.Collections.Generic;

namespace Highway.Data.Tests.Utilities.CloneExtension
{
    public class Employer
    {
        public Employee Ceo { get; set; }
        public List<Employee> Employees { get; set; }
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
        }

        [TestMethod]
        public void ShouldNotThrowExceptionWithNullReferences()
        {
            var employer = new Employer { Ceo = null };

            var employerClone = employer.Clone();
            
            Assert.IsNull(employerClone.Ceo);
        }

        [TestMethod]
        public void ShouldCloneCollection()
        {
            var employer = new Employer
            {
                Ceo = new Employee(),
                Employees = new List<Employee>
                {
                    new Employee{FirstName="John"},
                    new Employee()
                }
            };

            var employerClone = employer.Clone();

            Assert.AreEqual(2, employerClone.Employees.Count);
            Assert.AreEqual("John", employerClone.Employees.First().FirstName);
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
