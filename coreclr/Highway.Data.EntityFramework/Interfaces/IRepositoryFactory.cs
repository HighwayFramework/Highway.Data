using System;
using System.Linq;

namespace Highway.Data
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