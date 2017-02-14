using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

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
			return this.SaveChanges();
		}

		public Task<int> CommitAsync()
		{
			return this.SaveChangesAsync();
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
	}
}
