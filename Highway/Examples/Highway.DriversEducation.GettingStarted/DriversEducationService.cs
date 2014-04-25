#region

using System.Collections.Generic;
using System.Linq;
using Highway.Data;

#endregion

namespace Highway.DriversEducation.GettingStarted
{
    public class DriversEducationService
    {
        private readonly IRepository _repository;

        public DriversEducationService(IRepository repository)
        {
            _repository = repository;
        }

        public void AddDriver(string name)
        {
            _repository.Context.Add(new Driver {LastName = name});
            _repository.Context.Commit();
        }

        public Driver GetDriver(string name)
        {
            return _repository.Find(new DriverByName(name));
        }

        public void RemoveDriversByName(string name)
        {
            _repository.Execute(new RemoveDrivers(name));
        }
    }

    public class SwapInstructors : Scalar<int>
    {
        public SwapInstructors(Instructor currentInstructor, Instructor newInstructor)
        {
            ContextQuery = context =>
            {
                foreach (var driver in currentInstructor.Drivers)
                {
                    driver.Instructor = newInstructor;
                }
                return context.Commit();
            };
        }
    }


    public class RemoveDrivers : Command
    {
        public RemoveDrivers(string name)
        {
            ContextQuery = c =>
            {
                foreach (var driver in c.AsQueryable<Driver>().Where(x => x.LastName == name))
                {
                    c.Remove(driver);
                }
                c.Commit();
            };
        }
    }

    public class DriverByName : Scalar<Driver>
    {
        public DriverByName(string name)
        {
            ContextQuery = c => c.AsQueryable<Driver>().Single(x => x.LastName == name);
        }
    }

    public class DriversByName : Query<Driver>
    {
        public DriversByName(string name)
        {
            ContextQuery = c => c.AsQueryable<Driver>().Where(x => x.LastName == name);
        }
    }

    public class Driver
    {
        public Driver()
        {
        }

        public Driver(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Score { get; set; }
        public Instructor Instructor { get; set; }
    }

    public class Instructor
    {
        public int Id { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}