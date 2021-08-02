using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public class ReadonlyRepository : IReadonlyRepository
    {
        protected ReadonlyRepository(IReadonlyDataContext context)
        {
            Context = context;
        }

        public virtual event EventHandler<BeforeQuery> BeforeQuery;

        public virtual event EventHandler<BeforeScalar> BeforeScalar;

        public virtual event EventHandler<AfterQuery> AfterQuery;

        public virtual event EventHandler<AfterScalar> AfterScalar;

        /// <summary>
        ///     Reference to the DataContext the repository is using
        /// </summary>
        public IReadonlyDataContext Context { get; }

        /// <summary>
        ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The instance of <typeparamref name="T" /> returned from the query</returns>
        public virtual T Find<T>(IScalar<T> query)
        {
            OnBeforeScalar(new BeforeScalar(query));
            var result = query.Execute(Context);
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
            var result = query.Execute(Context);
            OnAfterQuery(new AfterQuery(result));

            return result;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
        public virtual IEnumerable<TProjection> Find<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
            where TSelection : class
        {
            OnBeforeQuery(new BeforeQuery(query));
            var results = query.Execute(Context);
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
            var task = new Task<T>(() => query.Execute(Context));
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
            var task = new Task<IEnumerable<T>>(() => query.Execute(Context));
            task.Start();

            return task;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
        public virtual Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
            where TSelection : class
        {
            var task = new Task<IEnumerable<TProjection>>(() => query.Execute(Context));
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
