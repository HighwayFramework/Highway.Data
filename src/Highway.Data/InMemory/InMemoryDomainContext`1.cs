namespace Highway.Data.Contexts
{
    public class InMemoryDomainContext<T> : InMemoryDataContext, IDomainContext<T> where T : class
    {
    }
}