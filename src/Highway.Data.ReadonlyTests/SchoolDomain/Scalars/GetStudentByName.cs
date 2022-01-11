using System.Data.Entity;
using System.Linq;

namespace Highway.Data.ReadonlyTests.SchoolDomain
{
    public class GetStudentByName : Scalar<Student>
    {
        public GetStudentByName(string name)
        {
            ContextQuery = source => source.AsQueryable<Student>()
                                           .Include(x => x.Grade)
                                           .Single(x => x.Name == name);
        }
    }
}
