namespace Highway.Data.Interfaces
{
    /// <summary>
    /// Factory to create repositories, both wide repositories and aggregate root repositories
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparam name="TOne"></typeparam> loaded
        /// </summary>
        /// <typeparam name="TOne">The aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparam name="TOne"></typeparam> </returns>
        IRepository Create<TOne>();

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparam name="TOne"></typeparam>,<typeparam name="TTwo"></typeparam> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparam name="TOne"></typeparam> </returns>
        IRepository Create<TOne, TTwo>();

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparam name="TOne"></typeparam>,<typeparam name="TTwo"></typeparam>,<typeparam name="TThree"></typeparam> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TThree">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparam name="TOne"></typeparam> </returns>
        IRepository Create<TOne, TTwo, TThree>();

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparam name="TOne"></typeparam>,<typeparam name="TTwo"></typeparam>,<typeparam name="TThree"></typeparam>,<typeparam name="TFour"></typeparam> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TThree">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TFour">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparam name="TOne"></typeparam>,<typeparam name="TTwo"></typeparam>,<typeparam name="TThree"></typeparam>,<typeparam name="TFour"></typeparam> </returns>
        IRepository Create<TOne, TTwo, TThree, TFour>();
    }
}