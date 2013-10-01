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
