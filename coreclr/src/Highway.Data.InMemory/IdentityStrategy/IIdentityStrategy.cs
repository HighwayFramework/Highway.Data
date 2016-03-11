namespace Highway.Data.InMemory
{
    public interface IIdentityStrategy<T>
        where T : class
    {
        void Assign(T entity);
    }
}
