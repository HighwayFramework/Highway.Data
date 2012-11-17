using System.Data.SqlClient;
using System.Linq;
using Highway.Data.QueryObjects;

namespace Highway.Data.GettingStarted
{
    public class FindPersonByLastName : Query<Person>
    {
        public FindPersonByLastName(string lastName)
        {
            //LINQ 
            //ContextQuery = c => c.AsQueryable<Person>().Where(x => x.LastName == lastName);
            
            //StoredProcedure
            ContextQuery = c => c.ExecuteSqlQuery<Person>("exec FindPersonByLastName @lastName", new SqlParameter("lastName", lastName)).AsQueryable();

        }
    }

    public class GetPersonByFirstAndLastName : Scalar<Person>
    {
        public GetPersonByFirstAndLastName(string firstName,string lastName)
        {
            ContextQuery = c => c.AsQueryable<Person>().FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
        }
    }

    public class ChangeStatusOnAllPeople : Command
    {
        public ChangeStatusOnAllPeople(int recordsToUpdate)
        {
            ContextQuery =
                c => c.ExecuteSqlCommand("exec updateStatus @rowCount", new SqlParameter("rowCount", recordsToUpdate));
        }
    }
}
