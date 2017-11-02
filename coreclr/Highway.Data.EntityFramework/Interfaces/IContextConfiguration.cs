

using System.Data.Entity;

namespace Highway.Data
{
	/// <summary>
	///     Implement this interface to pass the context specific mapping to the constructor
	/// </summary>
	public interface IContextConfiguration
	{
		/// <summary>
		///     This method allows the configuration of context specific properties to be injected
		/// </summary>
		/// <param name="context">the context that is being configured</param>
		void ConfigureContext(DbContext context);
	}
}