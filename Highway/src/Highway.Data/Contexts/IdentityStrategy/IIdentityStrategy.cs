namespace Highway.Data.Contexts
{
    public interface IIdentityStrategy<T>
        where T : class
    {
        void Assign(T entity);
    }
}