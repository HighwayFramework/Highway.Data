using System;
using System.Linq;
using Highway.Data.EntityFramework.Castle.Example.Domain;
using Highway.Data.EntityFramework.Castle.Example.Queries;
using Highway.Data.Interfaces;

namespace Highway.Data.EntityFramework.Castle.Example
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

            Person person = repo.Find(new LastNameQuery("Rayburn")).FirstOrDefault();

            Console.WriteLine("{0} thinks Highway.Data Rocks!!!", person.FullName);
        }

        private void AddPerson()
        {
            // Adding a person to the repository
            repo.Context.Add(new Person {FirstName = "Tim", LastName = "Rayburn"});
            repo.Context.Commit();
        }
    }
}