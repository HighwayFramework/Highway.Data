using Highway.Data.Interceptors.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Data
{
    /// <summary>
    ///     A Entity Framework Specific repository implementation that uses Specification pattern to execute Queries in a
    ///     controlled fashion.
    /// </summary>
    public class Repository : IRepository
    {
        private readonly IDataContext _context;

        /// <summary>
        ///     Creates a Repository that uses the context provided
        /// </summary>
        /// <param name="context">The data context that this repository uses</param>
        public Repository(IDataContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Reference to the DataContext the repository is using
        /// </summary>
        public IUnitOfWork Context => _context;

        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" />
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        public virtual void Execute(ICommand command)
        {
            OnBeforeCommand(new BeforeCommand(command));
            command.Execute(_context);
            OnAfterCommand(new AfterCommand(command));
        }

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
        /// <typeparam name="TSelection">The Entity being queried from the data store.</typeparam>
        /// <typeparam name="TProjection">The type being returned to the caller.</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
        public virtual IEnumerable<TProjection> Find<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
            where TSelection : class
        {
            OnBeforeQuery(new BeforeQuery(query));
            var results = query.Execute(_context);
            OnAfterQuery(new AfterQuery(results));
            return results;
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" /> asynchronously
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        public virtual Task ExecuteAsync(ICommand command)
        {
            var task = new Task(() => command.Execute(_context));
            task.Start();
            return task;
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
        /// <typeparam name="TSelection">The Entity being queried from the data store.</typeparam>
        /// <typeparam name="TProjection">The type being returned to the caller.</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
        public virtual Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
            where TSelection : class
        {
            var task = new Task<IEnumerable<TProjection>>(() => query.Execute(_context));
            task.Start();
            return task;
        }

        public event EventHandler<BeforeQuery> BeforeQuery;

        protected virtual void OnBeforeQuery(BeforeQuery e)
        {
            BeforeQuery?.Invoke(this, e);
        }

        public event EventHandler<BeforeScalar> BeforeScalar;

        protected virtual void OnBeforeScalar(BeforeScalar e)
        {
            BeforeScalar?.Invoke(this, e);
        }

        public event EventHandler<BeforeCommand> BeforeCommand;

        protected virtual void OnBeforeCommand(BeforeCommand e)
        {
            BeforeCommand?.Invoke(this, e);
        }

        public event EventHandler<AfterQuery> AfterQuery;

        protected virtual void OnAfterQuery(AfterQuery e)
        {
            AfterQuery?.Invoke(this, e);
        }

        public event EventHandler<AfterScalar> AfterScalar;

        protected virtual void OnAfterScalar(AfterScalar e)
        {
            AfterScalar?.Invoke(this, e);
        }

        public event EventHandler<AfterCommand> AfterCommand;

        protected virtual void OnAfterCommand(AfterCommand e)
        {
            AfterCommand?.Invoke(this, e);
        }
    }
}