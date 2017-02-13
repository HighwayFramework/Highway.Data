
using System;
using Highway.Data.Interceptors.Events;


namespace Highway.Data
{
	/// <summary>
	///     The interface used to surface events for queries that support interceptors
	/// </summary>
	public interface IObservableQuery
	{
		/// <summary>
		///     The event fired just before the query goes to the database
		/// </summary>
		event EventHandler<BeforeQuery> BeforeQuery;

		/// <summary>
		///     The event fire just after the data is translated into objects but before the data is returned.
		/// </summary>
		event EventHandler<AfterQuery> AfterQuery;
	}
}