namespace Highway.Data
{
    public interface IReadonlyDomainContext<in T> : IReadonlyDataContext where T : class
    {
    }
}