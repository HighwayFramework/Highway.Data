using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highway.Data.EntityFramework;
using Highway.Data.GettingStarted.DataAccess.Mappings;
using Highway.Data.GettingStarted.DataAccess.Queries;
using Highway.Data.GettingStarted.Domain.Entities;
using Highway.Data.QueryObjects;

namespace Highway.Data.GettingStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup
            var context = new DataContext("Data Source=(local);Initial Catalog=GettingStarted;Integrated Security=true;",new GettingStartedMappings());
            var repository = new Repository(context);

            //Adding Items
            repository.Context.Add(new Person() {FirstName = "Devlin", LastName = "Liles"});
            repository.Context.Add(new Person() { FirstName = "Tim", LastName = "Rayburn" });
            repository.Context.Add(new Person() { FirstName = "Jay", LastName = "Smith" });
            repository.Context.Add(new Person() { FirstName = "Brian", LastName = "Sullivan" });

            repository.Context.Commit();

            IEnumerable<Person> results = repository.Find(new FindPersonByLastName("Rayburn"));
            var person = results.First();
            person.LastName = "SillyRayburn"; //HEHEHE - He is not going to like this.

            repository.Context.Commit();

            repository.Get(new GetPersonByFirstAndLastName("Devlin", "Liles"));
            repository.Execute(new ChangeStatusOnAllPeople(3));
        }
    }
}
