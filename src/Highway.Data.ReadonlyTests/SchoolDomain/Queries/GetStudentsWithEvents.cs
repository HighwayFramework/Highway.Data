using System.Data.Entity;

namespace Highway.Data.ReadonlyTests
{
    internal class GetStudentsWithEvents : QueryWithEvents<Student>
    {
        public GetStudentsWithEvents()
        {
            ContextQuery = source => source.AsQueryable<Student>().Include(x => x.Grade);
        }
    }
}