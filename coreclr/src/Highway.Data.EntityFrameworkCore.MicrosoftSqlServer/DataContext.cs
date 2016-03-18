using System;
using System.Data.Common;
using Highway.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;

namespace Highway.Data.EntityFrameworkCore.MicrosoftSqlServer
{
    public class DataContext : DbContext, IEntityDataContext
    {
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.Set<T>();
        }

        public T Add<T>(T item) where T : class
        {
            return this.Add(item);
        }

        public new T Remove<T>(T item) where T : class
        {
            base.Remove(item);
            return item;
        }

        public T Update<T>(T item) where T : class
        {
            var entry = GetChangeTrackingEntry(item);
            if (entry == null)
            {
                throw new InvalidOperationException(
                    "Cannot Update an object that is not attacched to the current Entity Framework data context");
            }
            entry.State = EntityState.Modified;
            return item;
        }

        public T Reload<T>(T item) where T : class
        {
            // EF Core hasn't implemented this feature yet.
            // So neither can we.
            throw new NotImplementedException();
        }

        public int Commit()
        {
            return this.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return this.SaveChangesAsync();
        }

        public T Attach<T>(T item) where T : class
        {
            Set<T>().Attach(item);
            return item;
        }

        public T Detach<T>(T item) where T : class
        {
            var entry = GetChangeTrackingEntry(item);
            if (entry == null)
            {
                throw new InvalidOperationException(
                    "Cannot detach an object that is not attached to the current context.");
            }
            entry.State = EntityState.Detached;
            return item;
        }

        protected virtual EntityEntry<T> GetChangeTrackingEntry<T>(T item) where T : class
        {
            var entry = Entry(item);
            return entry;
        }
    }
}
