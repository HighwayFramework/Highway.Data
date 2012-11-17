using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highway.Data.GettingStarted.DataAccess.Mappings;
using Highway.Data.GettingStarted.Domain.Entities;

namespace Highway.Data.GettingStarted.AdvancedQueries
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new DataContext("Data Source=(local);Initial Catalog=GettingStarted;Integrated Security=true;", new GettingStartedMappings());
            var repository = new Repository(context);

            IEnumerable<Person> results = repository.Find(new FindPersonByLastNameEmbeddedSQLQuery("Liles"));
        }
    }
}
