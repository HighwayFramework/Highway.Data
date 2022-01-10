using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data.Factories
{
    public class ReadonlyRepositoryFactory : IReadonlyRepositoryFactory
    {
        private readonly string _connectionString;

        private readonly IContextConfiguration _contextConfig;

        private readonly ILog _logger;

        private readonly IMappingConfiguration _mappings;

        /// <summary>
        ///     Creates a repository factory for the supplied list of domains
        /// </summary>
        public ReadonlyRepositoryFactory(string connectionString, IMappingConfiguration mappings)
            : this(connectionString, mappings, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Creates a repository factory for the supplied list of domains
        /// </summary>
        public ReadonlyRepositoryFactory(string connectionString, IMappingConfiguration mappings, IContextConfiguration contextConfiguration)
            : this(connectionString, mappings, contextConfiguration, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Creates a repository factory for the supplied list of domains
        /// </summary>
        public ReadonlyRepositoryFactory(string connectionString, IMappingConfiguration mappings, ILog logger)
            : this(connectionString, mappings, new DefaultContextConfiguration(), logger)
        {
        }

        /// <summary>
        ///     Creates a repository factory for the supplied list of domains
        /// </summary>
        public ReadonlyRepositoryFactory(string connectionString, IMappingConfiguration mappings, IContextConfiguration contextConfig, ILog logger)
        {
            _connectionString = connectionString;
            _contextConfig = contextConfig;
            _logger = logger;
        }

        /// <summary>
        ///     Creates a repository for the requested domain
        /// </summary>
        /// <returns>Domain specific repository</returns>
        public IReadonlyRepository CreateReadonly()
        {
            return new ReadonlyRepository(new ReadonlyDataContext(_connectionString, _mappings, _contextConfig, _logger));
        }
    }
}
