using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
			OnBeforeSave();
			var changes = this.SaveChanges();
			OnAfterSave();
			return changes;
		}

		public Task<int> CommitAsync()
		{
			OnBeforeSave();
			return this.SaveChangesAsync().ContinueWith(task => {
				OnAfterSave();
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

		private void OnAfterSave()
		{
			if (AfterSave != null) AfterSave(this, new AfterSave());
		}

		private void OnBeforeSave()
		{
			if (BeforeSave != null) BeforeSave(this, new BeforeSave());
		}


		public event EventHandler<BeforeSave> BeforeSave;
		public event EventHandler<AfterSave> AfterSave;
	}
}
