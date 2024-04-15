namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentWithReadonlyChild
{
    public class GetParents : Query<Parent>
    {
        public GetParents()
        {
            ContextQuery = dataSource => dataSource.AsQueryable<Parent>();
        }
    }
}
