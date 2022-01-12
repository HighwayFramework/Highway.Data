using System.Data.Entity;

namespace Highway.Data.ReadonlyTests
{
    internal class GetStudents : Query<Student>
    {
        public GetStudents()
        {
            ContextQuery = source => source.AsQueryable<Student>().Include(x => x.Grade);
        }
    }
}
