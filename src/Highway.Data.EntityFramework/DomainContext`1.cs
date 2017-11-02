using Common.Logging;
using Common.Logging.Simple;
using System;
using System.Linq;

namespace Highway.Data
{
	/// <summary>
	/// A Context that is constrained to a specified Domain
	/// </summary>
	/// <typeparam name="T">The Domain this context is specific for</typeparam>
	public class DomainContext<T> : DataContext, IDomainContext<T>
		where T : class, IDomain
	{
		private readonly T domain;

		/// <summary>
		/// Constructs the domain context
		/// </summary>
		/// <param name="domain"></param>
		public DomainContext(T domain)
			: base(domain.ConnectionString, domain.Mappings, domain.Context, new NoOpLogger())
		{
			this.domain = domain;
		}

		/// <summary>
		/// Constructs the domain context
		/// </summary>
		/// <param name="domain">domain for context</param>
		/// <param name="logger">logger</param>
		public DomainContext(T domain, ILog logger)
			: base(domain.ConnectionString, domain.Mappings, domain.Context, logger)
		{
			this.domain = domain;
		}

		public T Domain => domain;
	}
}