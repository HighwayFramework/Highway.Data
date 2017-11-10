using Highway.Data.Interceptors.Events;
using System;

namespace Highway.Data
{
	/// <summary>
	/// This is the interface to observe when a Commit is made to the database.
	/// </summary>
	public interface IObservableUnitOfWork : IUnitOfWork
	{
		/// <summary>
		///     The event fired just before the commit of the persistence
		/// </summary>
		event EventHandler<BeforeCommit> BeforeCommit;

		/// <summary>
		///     The event fired just after the commit of the persistence
		/// </summary>
		event EventHandler<AfterCommit> AfterCommit;
	}
}