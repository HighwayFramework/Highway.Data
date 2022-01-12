namespace Highway.Data
{
    /// <summary>
    ///     Contract for a readonly Domain Context
    /// </summary>
    /// <typeparam name="T">The type of the Entity</typeparam>
    public interface IReadonlyDomainContext<in T> : IReadonlyDataContext
        where T : class
    {
    }
}
