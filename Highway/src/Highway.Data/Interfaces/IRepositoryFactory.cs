namespace Highway.Data.Interfaces
{
    /// <summary>
    /// Factory to create repositories, both wide repositories and aggregate root repositories
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/> loaded
        /// </summary>
        /// <typeparam name="TOne">The aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/> </returns>
        IRepository Create<TOne>();

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/>,<typeparamref name="TTwo"/> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/> </returns>
        IRepository Create<TOne, TTwo>();

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>,<typeparamref name="TThree"/> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TThree">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/> </returns>
        IRepository Create<TOne, TTwo, TThree>();

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>,<typeparamref name="TThree"/>,<typeparamref name="TFour"/> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TThree">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TFour">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>,<typeparamref name="TThree"/>,<typeparamref name="TFour"/> </returns>
        IRepository Create<TOne, TTwo, TThree, TFour>();
    }
}