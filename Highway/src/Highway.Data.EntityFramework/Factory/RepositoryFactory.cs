using System;
using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Repositories;
using Highway.Data.Interfaces;

namespace Highway.Data.EntityFramework.Factory
{
    /// <summary>
    /// The Entity Framework Specific Repository Implementation
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string _connectionString;
        private readonly Func<Type, IMappingConfiguration> _mappingsDelegate;

        /// <summary>
        /// Creates a repository factory with a connection string to the database being talked to
        /// </summary>
        /// <param name="connectionString">The SQL Connection string for the context</param>
        /// <param name="mappingsDelegate"></param>
        public RepositoryFactory(string connectionString, Func<Type,IMappingConfiguration> mappingsDelegate)
        {
            _connectionString = connectionString;
            _mappingsDelegate = mappingsDelegate;
        }

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/> loaded
        /// </summary>
        /// <typeparam name="TOne">The aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/> </returns>
        public IRepository Create<TOne>()
        {
            var mappings = new[]
                               {
                                   GetMapping(typeof (TOne))
                               };

            return CreateRepository(mappings);
        }

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/>,<typeparamref name="TTwo"/> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/> and <typeparamref name="TTwo"/> </returns>
        public IRepository Create<TOne, TTwo>()
        {
            var mappings = new[]
                               {
                                   GetMapping(typeof (TOne)),
                                   GetMapping(typeof (TTwo))
                               };

            return CreateRepository(mappings);
        }

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>,<typeparamref name="TThree"/> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TThree">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>, and <typeparamref name="TThree"/> </returns>
        public IRepository Create<TOne, TTwo, TThree>()
        {
            var mappings = new[]
                               {
                                   GetMapping(typeof (TOne)),
                                   GetMapping(typeof (TTwo)),
                                   GetMapping(typeof (TThree))
                               };
            return CreateRepository(mappings);
        }

        /// <summary>
        /// Creates a repository with a context loaded with mappings of the aggregate root <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>,<typeparamref name="TThree"/>,<typeparamref name="TFour"/> loaded
        /// </summary>
        /// <typeparam name="TOne">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TTwo">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TThree">An aggregate root type that the context is being configured for</typeparam>
        /// <typeparam name="TFour">An aggregate root type that the context is being configured for</typeparam>
        /// <returns>A repository configured to <typeparamref name="TOne"/>,<typeparamref name="TTwo"/>,<typeparamref name="TThree"/>, and <typeparamref name="TFour"/> </returns>
        public IRepository Create<TOne, TTwo, TThree, TFour>()
        {
            var mappings = new[]
                               {
                                   GetMapping(typeof (TOne)),
                                   GetMapping(typeof (TTwo)),
                                   GetMapping(typeof (TThree)),
                                   GetMapping(typeof (TFour))
                               };

            return CreateRepository(mappings);
        }

        private Repository CreateRepository(IMappingConfiguration[] mappings)
        {
            return new Repository(new Context(_connectionString, mappings));
        }

        private IMappingConfiguration GetMapping(Type type)
        {
            return _mappingsDelegate(type);
        }
    }
}