using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highway.Data.Filtering;
using Highway.Data.GettingStarted.DataAccess.Mappings;
using Highway.Data.GettingStarted.DataAccess.Queries;
using Highway.Data.GettingStarted.Domain.Entities;
using Highway.Data.Interfaces;
using Highway.Data.QueryObjects;
using System.Linq.Dynamic;

namespace Highway.Data.GettingStarted.DynamicFiltering
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new DataContext("Data Source=(local);Initial Catalog=GettingStarted;Integrated Security=true;", new GettingStartedMappings());
            var repository = new Repository(context);

            //Adding Items
            repository.Context.Add(new Person() { FirstName = "Devlin", LastName = "Liles" });
            repository.Context.Add(new Person() { FirstName = "Tim", LastName = "Rayburn" });
            repository.Context.Add(new Person() { FirstName = "Jay", LastName = "Smith" });
            repository.Context.Add(new Person() { FirstName = "Brian", LastName = "Sullivan" });

            repository.Context.Commit();
            var criteria = Criteria.Field<DateTime>("BirthDate").On(new DateTime(2012,1,1));
            
            IEnumerable<Person> results = repository.Find(new FindByCriteria<Person>(criteria));
            var person = results.First();

            var secondResults = results.Where("FirstName = @0", "Devlin");

        }
    }

}
