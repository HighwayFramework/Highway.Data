namespace Highway.Data
{
    /// <summary>
    ///     Contract for a Domain Context
    /// </summary>
    /// <typeparam name="T">The type of the Entity</typeparam>
    public interface IDomainContext<in T> : IReadonlyDomainContext<T>, IDataContext
        where T : class
    {
    }
}
