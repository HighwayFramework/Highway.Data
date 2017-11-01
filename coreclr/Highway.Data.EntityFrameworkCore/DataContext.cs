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
			var changes = this.SaveChanges();
			AfterSave?.Invoke(this, new AfterSave());
			return changes;
		}

		public Task<int> CommitAsync()
		{
			BeforeSave?.Invoke(this, new BeforeSave());
			return this.SaveChangesAsync().ContinueWith(task => {
				AfterSave?.Invoke(this, new AfterSave());
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
