using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        public IUnitOfWork Context
        {
            get { return _context; }
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" />
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        public virtual void Execute(ICommand command)
        {
            command.Execute(_context);
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The instance of <typeparamref name="T" /> returned from the query</returns>
        public virtual T Find<T>(IScalar<T> query)
        {
            return query.Execute(_context);
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
        public virtual IEnumerable<T> Find<T>(IQuery<T> query)
        {
            return query.Execute(_context);
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
        public virtual IEnumerable<IProjection> Find<TSelection, IProjection>(IQuery<TSelection, IProjection> query)
            where TSelection : class
        {
            return query.Execute(_context);
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
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
        public virtual Task<IEnumerable<IProjection>> FindAsync<TSelection, IProjection>(IQuery<TSelection, IProjection> query)
            where TSelection : class
        {
            var task = new Task<IEnumerable<IProjection>>(() => query.Execute(_context));
            task.Start();
            return task;
        }
    }
}