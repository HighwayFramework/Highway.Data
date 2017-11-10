using Highway.Data.Interceptors.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Highway.Data
{
	public class ObservableUnitOfWorkDecorator : IObservableUnitOfWork
	{
		private readonly IUnitOfWork uow;
		public ObservableUnitOfWorkDecorator(IUnitOfWork uow)
		{
			this.uow = uow;
		}

		public event EventHandler<BeforeCommit> BeforeCommit;
		public event EventHandler<AfterCommit> AfterCommit;

		public T Add<T>(T item) where T : class
		{
			return uow.Add<T>(item);
		}

		public IQueryable<T> AsQueryable<T>() where T : class
		{
			return uow.AsQueryable<T>();
		}

		public int Commit()
		{
			BeforeCommit?.Invoke(this, new BeforeCommit());

			int? changes = null;
			Exception exception = null;

			try
			{
				changes = uow.Commit();
			}
			catch (Exception ex)
			{
				exception = ex;
				AfterCommit?.Invoke(this, new AfterCommit(changes, exception));
				throw; // AfterSave might change the exception and throw, if not, do it ourself
			}
			finally
			{
				AfterCommit?.Invoke(this, new AfterCommit(changes, exception));
			}

			return changes.Value;
		}

		public Task<int> CommitAsync()
		{
			BeforeCommit?.Invoke(this, new BeforeCommit());
			return uow.CommitAsync()
				.ContinueWith(task =>
				{
					int? changes = task.Status == TaskStatus.RanToCompletion ? new int?(task.Result) : null;
					Exception ex = task.Exception;
					AfterCommit?.Invoke(this, new AfterCommit(changes, ex));

					if (ex != null) throw ex;
					else return task.Result;
				});
		}

		public IUnitOfWork GetRootDecoratedObject()
		{
			return uow.GetRootDecoratedObject();
		}

		public T Reload<T>(T item) where T : class
		{
			return uow.Reload<T>(item);
		}

		public T Remove<T>(T item) where T : class
		{
			return uow.Remove<T>(item);
		}

		public T Update<T>(T item) where T : class
		{
			return uow.Update<T>(item);
		}
	}
}
