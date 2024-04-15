namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentWithReadonlyChild
{
    public class GetChildren : Query<Child>
    {
        public GetChildren()
        {
            ContextQuery = dataSource => dataSource.AsQueryable<Child>();
        }
    }
}