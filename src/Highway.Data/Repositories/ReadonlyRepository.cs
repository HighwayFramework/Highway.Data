using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public class ReadonlyRepository : IReadonlyRepository
    {
        private readonly IReadonlyDataContext _context;

        protected ReadonlyRepository(IReadonlyDataContext context)
        {
            _context = context;
        }

        public virtual event EventHandler<BeforeQuery> BeforeQuery;

        public virtual event EventHandler<BeforeScalar> BeforeScalar;

        public virtual event EventHandler<AfterQuery> AfterQuery;

        public virtual event EventHandler<AfterScalar> AfterScalar;

        /// <summary>
        ///     Reference to the DataContext the repository is using
        /// </summary>
        public IUnitOfWork Context => _context;

        /// <summary>
        ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The instance of <typeparamref name="T" /> returned from the query</returns>
        public virtual T Find<T>(IScalar<T> query)
        {
            OnBeforeScalar(new BeforeScalar(query));
            var result = query.Execute(_context);
            OnAfterScalar(new AfterScalar(query));

            return result;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
        public virtual IEnumerable<T> Find<T>(IQuery<T> query)
        {
            OnBeforeQuery(new BeforeQuery(query));
            var result = query.Execute(_context);
            OnAfterQuery(new AfterQuery(result));

            return result;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
        public virtual IEnumerable<TProjector> Find<TSelector, TProjector>(IQuery<TSelector, TProjector> query)
            where TSelector : class
        {
            OnBeforeQuery(new BeforeQuery(query));
            var results = query.Execute(_context);
            OnAfterQuery(new AfterQuery(results));

            return results;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return an instance of <typeparamref name="T" /> from the query</returns>
        public virtual Task<T> FindAsync<T>(IScalar<T> query)
        {
            var task = new Task<T>(() => query.Execute(_context));
            task.Start();

            return task;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
        public virtual Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
        {
            var task = new Task<IEnumerable<T>>(() => query.Execute(_context));
            task.Start();

            return task;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
        public virtual Task<IEnumerable<TProjector>> FindAsync<TSelector, TProjector>(IQuery<TSelector, TProjector> query)
            where TSelector : class
        {
            var task = new Task<IEnumerable<TProjector>>(() => query.Execute(_context));
            task.Start();

            return task;
        }

        protected virtual void OnAfterQuery(AfterQuery e)
        {
            AfterQuery?.Invoke(this, e);
        }

        protected virtual void OnAfterScalar(AfterScalar e)
        {
            AfterScalar?.Invoke(this, e);
        }

        protected virtual void OnBeforeQuery(BeforeQuery e)
        {
            BeforeQuery?.Invoke(this, e);
        }

        protected virtual void OnBeforeScalar(BeforeScalar e)
        {
            BeforeScalar?.Invoke(this, e);
        }
    }
}
