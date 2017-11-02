using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Highway.Data.Interceptors.Events;
using Highway.Data.InMemory;

namespace Highway.Data
{
	public class InMemoryDataContext : IDataContext
	{
		internal readonly ObjectRepresentationRepository repo;
		private readonly Queue<object> addQueue = new Queue<object>();
		private readonly Queue<object> removeQueue = new Queue<object>();

		public event EventHandler<BeforeSave> BeforeSave;
		public event EventHandler<AfterSave> AfterSave;

		public InMemoryDataContext()
		{
			repo = new ObjectRepresentationRepository();
			RegisterIIdentifiables();
		}

		internal InMemoryDataContext(ObjectRepresentationRepository repo)
		{
			this.repo = repo;
			RegisterIIdentifiables();
		}

		private void RegisterIIdentifiables()
		{
			RegisterIdentityStrategy(new IntegerIdentityStrategy<IIdentifiable<int>>(x => x.Id));
			RegisterIdentityStrategy(new ShortIdentityStrategy<IIdentifiable<short>>(x => x.Id));
			RegisterIdentityStrategy(new LongIdentityStrategy<IIdentifiable<long>>(x => x.Id));
			RegisterIdentityStrategy(new GuidIdentityStrategy<IIdentifiable<Guid>>(x => x.Id));
		}

		public void Dispose()
		{
		}

		public virtual IQueryable<T> AsQueryable<T>() where T : class
		{
			return repo.Data<T>();
		}

		public virtual T Add<T>(T item) where T : class
		{
			addQueue.Enqueue(item);
			return item;
		}

		public virtual T Remove<T>(T item) where T : class
		{
			removeQueue.Enqueue(item);
			return item;
		}

		public virtual T Update<T>(T item) where T : class
		{
			return item;
		}

		public virtual T Reload<T>(T item) where T : class
		{
			return item;
		}

		public virtual int Commit()
		{
			BeforeSave?.Invoke(this, new Interceptors.Events.BeforeSave());
			ProcessCommitQueues();
			repo.Commit();
			AfterSave?.Invoke(this, new Interceptors.Events.AfterSave());
			return 0;
		}

		public virtual Task<int> CommitAsync()
		{
			var task = new Task<int>(Commit);
			task.Start();
			return task;
		}

		/// <summary>
		/// This method allows you to register database "identity" like strategies for auto incrementing keys, or new guid keys, etc...
		/// </summary>
		/// <param name="identityStrategy">The strategy to use for an object</param>
		/// <typeparam name="T">The type to use it from</typeparam>
		public void RegisterIdentityStrategy<T>(IIdentityStrategy<T> identityStrategy) where T : class
		{
			if (repo.IdentityStrategies.ContainsKey(typeof(T)))
			{
				repo.IdentityStrategies[typeof(T)] = obj => identityStrategy.Assign((T)obj);
			}
			else
			{
				repo.IdentityStrategies.Add(typeof(T), obj => identityStrategy.Assign((T)obj));
			}
		}

		/// <summary>
		/// Processes the held but uncommitted adds and removes from the context
		/// </summary>
		protected void ProcessCommitQueues()
		{
			AddAllFromQueueIntoRepository();
			RemoveAllFromQueueFromRepository();
		}

		private void AddAllFromQueueIntoRepository()
		{
			while (addQueue.Count > 0)
			{
				repo.Add(addQueue.Dequeue());
			}
		}
		private void RemoveAllFromQueueFromRepository()
		{
			while (removeQueue.Count > 0)
			{
				repo.Remove(removeQueue.Dequeue());
			}
		}
	}
}