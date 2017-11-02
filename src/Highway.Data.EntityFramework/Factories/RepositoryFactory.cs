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
		private readonly IContextConfiguration _contextConfig;
		private readonly ILog _logger;

		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings)
			: this(connectionString, mappings, new DefaultContextConfiguration(), new NoOpLogger())
		{

		}
		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings, IContextConfiguration contextConfiguration)
			: this(connectionString, mappings, contextConfiguration, new NoOpLogger())
		{

		}
		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings, ILog logger)
			: this(connectionString, mappings, new DefaultContextConfiguration(), logger)
		{

		}
		/// <summary>
		/// Creates a repository factory for the supplied list of domains
		/// </summary>
		public RepositoryFactory(string connectionString, IMappingConfiguration mappings, IContextConfiguration contextConfig, ILog logger)
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
			return new Repository(new DataContext(_connectionString, _mappings, _contextConfig, _logger));
		}
	}
}