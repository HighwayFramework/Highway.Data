namespace Highway.Data.Factories
{
    /// <summary>
    /// Simple repository factory
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a repository for the requested domain
        /// </summary>
        /// <returns>Domain specific repository</returns>
        IRepository Create();
    }
}