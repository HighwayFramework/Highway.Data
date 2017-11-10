
using System;
using System.Linq;


namespace Highway.Data
{
	/// <summary>
	///     The standard interface used to interact with an ORM specific implementation
	/// </summary>
	public interface IReadOnlyUnitOfWork
	{
		/// <summary>
		///     This gives a mock-able wrapper around normal Set method that allows for test-ablity
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <returns>
		///     <see cref="IQueryable{T}" />
		/// </returns>
		IQueryable<T> AsQueryable<T>() where T : class;

		/// <summary>
		///     Updates the provided instance of <typeparamref name="T" /> in the data unit of work
		/// </summary>
		/// <typeparam name="T">The Entity Type being updated</typeparam>
		/// <param name="item">The <typeparamref name="T" /> you want to update</param>
		/// <returns>The <typeparamref name="T" /> you updated</returns>
		T Update<T>(T item) where T : class;

		/// <summary>
		///     Reloads the provided instance of <typeparamref name="T" /> from the database
		/// </summary>
		/// <typeparam name="T">The Entity Type being reloaded</typeparam>
		/// <param name="item">The <typeparamref name="T" /> you want to reload</param>
		/// <returns>The <typeparamref name="T" /> you reloaded</returns>
		T Reload<T>(T item) where T : class;

	}
}