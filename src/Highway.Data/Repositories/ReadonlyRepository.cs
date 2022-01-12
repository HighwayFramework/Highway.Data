using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public class ReadonlyRepository : IReadonlyRepository
    {
        public ReadonlyRepository(IReadonlyDataContext context)
        {
            Context = context;
        }

        public event EventHandler<BeforeQuery> BeforeQuery;

        public event EventHandler<BeforeScalar> BeforeScalar;

        public event EventHandler<AfterQuery> AfterQuery;

        public event EventHandler<AfterScalar> AfterScalar;

        protected IReadonlyDataContext Context { get; }

        /// <summary>
        ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="scalar">The prebuilt Query Object</param>
        /// <returns>The instance of <typeparamref name="T" /> returned from the scalar</returns>
        public virtual T Find<T>(IScalar<T> scalar)
        {
            OnBeforeScalar(new BeforeScalar(scalar));
            var result = scalar.Execute(Context);
            OnAfterScalar(new AfterScalar(scalar));

            return result;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the scalar</returns>
        public virtual IEnumerable<T> Find<T>(IQuery<T> query)
        {
            OnBeforeQuery(new BeforeQuery(query));
            var result = query.Execute(Context);
            OnAfterQuery(new AfterQuery(result));

            return result;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="scalar">The prebuilt Query Object</param>
        /// <returns>The task that will return an instance of <typeparamref name="T" /> from the scalar</returns>
        public virtual Task<T> FindAsync<T>(IScalar<T> scalar)
        {
            var task = new Task<T>(() => scalar.Execute(Context));
            task.Start();

            return task;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the scalar</returns>
        public virtual Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
        {
            var task = new Task<IEnumerable<T>>(() => query.Execute(Context));
            task.Start();

            return task;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="TSelection">The Entity being queried from the data store.</typeparam>
        /// <typeparam name="TProjection">The type being returned to the caller.</typeparam>
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
