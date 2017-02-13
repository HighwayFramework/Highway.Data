using System;
using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
	/// <summary>
	/// Contract for a Domain Context
	/// </summary>
	/// <typeparam name="T">The type of the Entity</typeparam>
	public interface IDomainContext<in T> : IDataContext where T : class
	{
		/// <summary>
		///     The event fired just before the commit of the persistence
		/// </summary>
		event EventHandler<BeforeSave> BeforeSave;

		/// <summary>
		///     The event fired just after the commit of the persistence
		/// </summary>
		event EventHandler<AfterSave> AfterSave;
	}
}