using System;
using System.Collections.Generic;
using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Repositories;
using Highway.Data.Interfaces;
using Microsoft.Practices.ServiceLocation;

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

        public IRepository Create<TOne>()
        {
            var mappings = new[]
                               {
                                   GetMapping(typeof (TOne))
                               };

            return CreateRepository(mappings);
        }

        private EntityFrameworkRepository CreateRepository(IMappingConfiguration[] mappings)
        {
            return new EntityFrameworkRepository(new EntityFrameworkContext(_connectionString, mappings));
        }

        private IMappingConfiguration GetMapping(Type type)
        {
            return _mappingsDelegate(type);
        }

        public IRepository Create<TOne, TTwo>()
        {
            var mappings = new[]
                               {
                                   GetMapping(typeof (TOne)),
                                   GetMapping(typeof (TTwo))
                               };

            return CreateRepository(mappings);
        }

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
    }
}