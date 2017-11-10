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
		private readonly IUnitOfWork uow;

		/// <summary>
		///     Creates a Repository that uses the context provided
		/// </summary>
		/// <param name="uow">The data context that this repository uses</param>
		public Repository(IUnitOfWork uow)
		{
			this.uow = uow;
		}

		/// <summary>
		///     Reference to the DataContext the repository is using
		/// </summary>
		public IWriteOnlyUnitOfWork UnitOfWork
		{
			get { return uow as IWriteOnlyUnitOfWork; }
		}

		/// <summary>
		///     Executes a prebuilt <see cref="ICommand" />
		/// </summary>
		/// <param name="command">The prebuilt command object</param>
		public virtual void Execute(ICommand command)
		{
			OnBeforeCommand(new BeforeCommand(command));
			command.Execute(uow);
			OnAfterCommand(new AfterCommand(command));
		}

		/// <summary>
		///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
		/// </summary>
		/// <typeparam name="T">The Entity being queried</typeparam>
		/// <param name="query">The prebuilt Query Object</param>
		/// <returns>The instance of <typeparamref name="T" /> returned from the query</returns>
		public virtual T Find<T>(IScalar<T> scalar)
		{
			OnBeforeScalar(new BeforeScalar(scalar));
			var result = scalar.Execute(uow);
			OnAfterScalar(new AfterScalar(result));
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
			var result = query.Execute(uow);
			OnAfterQuery(new AfterQuery(result));
			return result;
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
			OnBeforeQuery(new BeforeQuery(query));
			var results = query.Execute(uow);
			OnAfterQuery(new AfterQuery(results));
			return results;
		}

		/// <summary>
		///     Executes a prebuilt <see cref="ICommand" /> asynchronously
		/// </summary>
		/// <param name="command">The prebuilt command object</param>
		public virtual Task ExecuteAsync(ICommand command)
		{
			OnBeforeCommand(new BeforeCommand(command));
			var task = new Task(() => command.Execute(uow)).ContinueWith(t =>
			{
				OnAfterCommand(new AfterCommand(command));
			});
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
			OnBeforeScalar(new BeforeScalar(query));
			var task = new Task<T>(() => query.Execute(uow)).ContinueWith(t =>
			{
				OnAfterScalar(new AfterScalar(query));
				return t.Result;
			});
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
			OnBeforeQuery(new BeforeQuery(query));
			var task = new Task<IEnumerable<T>>(() => query.Execute(uow)).ContinueWith(t =>
			{
				OnAfterQuery(new AfterQuery(query));
				return t.Result;
			});
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
			OnBeforeQuery(new BeforeQuery(query));
			var task = new Task<IEnumerable<IProjection>>(() => query.Execute(uow)).ContinueWith(t =>
			{
				OnAfterQuery(new AfterQuery(query));
				return t.Result;
			});
			task.Start();
			return task;
		}


		public event EventHandler<BeforeQuery> BeforeQuery;

		protected virtual void OnBeforeQuery(BeforeQuery e)
		{
			EventHandler<BeforeQuery> handler = BeforeQuery;
			if (handler != null) handler(this, e);
		}

		public event EventHandler<BeforeScalar> BeforeScalar;

		protected virtual void OnBeforeScalar(BeforeScalar e)
		{
			EventHandler<BeforeScalar> handler = BeforeScalar;
			if (handler != null) handler(this, e);
		}

		public event EventHandler<BeforeCommand> BeforeCommand;

		protected virtual void OnBeforeCommand(BeforeCommand e)
		{
			EventHandler<BeforeCommand> handler = BeforeCommand;
			if (handler != null) handler(this, e);
		}

		public event EventHandler<AfterQuery> AfterQuery;

		protected virtual void OnAfterQuery(AfterQuery e)
		{
			EventHandler<AfterQuery> handler = AfterQuery;
			if (handler != null) handler(this, e);
		}

		public event EventHandler<AfterScalar> AfterScalar;

		protected virtual void OnAfterScalar(AfterScalar e)
		{
			EventHandler<AfterScalar> handler = AfterScalar;
			if (handler != null) handler(this, e);
		}

		public event EventHandler<AfterCommand> AfterCommand;

		protected virtual void OnAfterCommand(AfterCommand e)
		{
			EventHandler<AfterCommand> handler = AfterCommand;
			if (handler != null) handler(this, e);
		}
	}
}