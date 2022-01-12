namespace Highway.Data.Factories
{
    /// <summary>
    ///     Simple readonly repository factory
    /// </summary>
    public interface IReadonlyRepositoryFactory
    {
        /// <summary>
        ///     Creates a readonly repository for the requested domain
        /// </summary>
        /// <returns>Domain specific repository</returns>
        IReadonlyRepository CreateReadonly();
    }
}
