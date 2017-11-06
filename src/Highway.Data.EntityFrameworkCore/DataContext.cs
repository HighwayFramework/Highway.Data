using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
	public class DataContext : DbContext, IDataContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

		public IQueryable<T> AsQueryable<T>() where T : class
		{
			return this.Set<T>();
		}

		public int Commit()
		{
			BeforeSave?.Invoke(this, new BeforeSave());

			int? changes = null;
			Exception exception = null;

			try
			{
				changes = this.SaveChanges();
			}
			catch (Exception ex)
			{
				exception = ex;
				AfterSave?.Invoke(this, new AfterSave(changes, exception));
				throw; // AfterSave might change the exception and throw, if not, do it ourself
			}
			finally
			{
				AfterSave?.Invoke(this, new AfterSave(changes, exception));
			}

			return changes.Value;
		}

		public Task<int> CommitAsync()
		{
			BeforeSave?.Invoke(this, new BeforeSave());
			return this.SaveChangesAsync().ContinueWith(task => {
				int? changes = task.Status == TaskStatus.RanToCompletion ? new int?(task.Result) : null;
				AfterSave?.Invoke(this, new AfterSave(changes, task.Exception));
				return task.Result;
			});
		}

		public T Reload<T>(T item) where T : class
		{
			this.ChangeTracker.Entries<T>()
				.Single(ee => ee.Entity == item)
				.Reload();
			return item;
		}

		T IUnitOfWork.Add<T>(T item)
		{
			this.Add<T>(item);
			return item;
		}

		T IUnitOfWork.Remove<T>(T item)
		{
			this.Remove<T>(item);
			return item;
		}

		T IUnitOfWork.Update<T>(T item)
		{
			this.Update<T>(item);
			return item;
		}

		public event EventHandler<BeforeSave> BeforeSave;
		public event EventHandler<AfterSave> AfterSave;
	}
}
