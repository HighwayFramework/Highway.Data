using Highway.Data.EntityFramework.Unity.Example.Domain;
using Highway.Data.EntityFramework.Unity.Example.Queries;
using Highway.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.EntityFramework.Unity.Example
{
    public class DemoApplication
    {
        private readonly IRepository repo;

        public DemoApplication(IRepository repo)
        {
            this.repo = repo;
        }

        public void Run()
        {
            //AddPerson();

            var person = repo.Find(new LastNameQuery("Rayburn")).FirstOrDefault();

            Console.WriteLine("{0} thinks Highway.Data Rocks!!!", person.FullName);
        }

        private void AddPerson()
        {
            // Adding a person to the repository
            repo.Context.Add(new Person { FirstName = "Tim", LastName = "Rayburn" });
            repo.Context.Commit();
        }
    }
}