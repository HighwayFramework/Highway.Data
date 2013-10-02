using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highway.Data;

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
            _repository.Context.Add(new Driver(name));
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

    public class RemoveDrivers : Command
    {
        public RemoveDrivers(string name)
        {
            ContextQuery = c =>
            {
                foreach (var driver in c.AsQueryable<Driver>().Where(x=>x.Name == name))
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
            ContextQuery = c => c.AsQueryable<Driver>().Single(x => x.Name == name);
        }
    }

    public class DriversByName : Query<Driver>
    {
        public DriversByName(string name)
        {
            ContextQuery = c => c.AsQueryable<Driver>().Where(x => x.Name == name);
        }
    }

    public class Driver
    {
        public string Name { get; set; }

        public Driver(string name)
        {
            Name = name;
        }
    }
}
