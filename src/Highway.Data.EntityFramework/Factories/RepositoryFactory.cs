using System;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data.Factories
{
	/// <summary>
	/// Simple factory for constructing repositories
	/// </summary>
	public class RepositoryFactory : IRepositoryFactory
	{
		private readonly string _connectionString;
		private readonly IMappingConfiguration _mappings;
		private readonly IUnitOfWorkConfiguration _contextConfig;
		private readonly ILog _logger;

		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings)
			: this(connectionString, mappings, new DefaultUnitOfWorkConfiguration(), new NoOpLogger())
		{

		}
		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings, IUnitOfWorkConfiguration contextConfiguration)
			: this(connectionString, mappings, contextConfiguration, new NoOpLogger())
		{

		}
		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings, ILog logger)
			: this(connectionString, mappings, new DefaultUnitOfWorkConfiguration(), logger)
		{

		}
		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings, IUnitOfWorkConfiguration contextConfig, ILog logger)
		{
			_connectionString = connectionString;
			_mappings = mappings;
			_contextConfig = contextConfig;
			_logger = logger;
		}

		/// <summary>
		/// Creates a repository for the requested domain
		/// </summary>
		/// <returns>Domain specific repository</returns>
		public IRepository Create()
		{
			return new Repository(new UnitOfWork(_connectionString, _mappings, _contextConfig, _logger));
		}
	}
}