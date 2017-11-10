
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Highway.Data
{
	public interface IReadOnlyRepository
	{

		/// <summary>
		///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <param name="query">The prebuilt Query Object</param>
		/// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
		IEnumerable<T> Find<T>(IQuery<T> query);

		/// <summary>
		///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <param name="query">The prebuilt Query Object</param>
		/// <returns>The instance of <typeparamref name="T" /> returned from the query</returns>
		T Find<T>(IScalar<T> query);

		/// <summary>
		///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <param name="query">The prebuilt Query Object</param>
		/// <returns>The task that will return an instance of <typeparamref name="T" /> from the query</returns>
		Task<T> FindAsync<T>(IScalar<T> query);

		/// <summary>
		///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <param name="query">The prebuilt Query Object</param>
		/// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
		Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query);

		/// <summary>
		///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <param name="query">The prebuilt Query Object</param>
		/// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
		Task<IEnumerable<IProjection>> FindAsync<TSelection, IProjection>(IQuery<TSelection, IProjection> query)
			where TSelection : class;
	}
}