using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using FrameworkExtension.Core.EventManagement;
using FrameworkExtension.Core.Interceptors.Events;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Services;

namespace FrameworkExtension.Core.Contexts
{
    public class EntityFrameworkContext : DbContext, IObservableDataContext
    {
        
        public EntityFrameworkContext(string connectionString) : base(connectionString) { }
        
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.Set<T>();
        }

        public T Add<T>(T item) where T : class
        {
            this.Set<T>().Add(item);
            return item;
        }

        public T Remove<T>(T item) where T : class
        {
            this.Set<T>().Remove(item);
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

        public T Attach<T>(T item) where T : class
        {
            this.Set<T>().Attach(item);
            return item;
        }

        public T Detach<T>(T item) where T : class
        {
            var entry = GetChangeTrackingEntry(item);
            if (entry == null)
            {
                throw new InvalidOperationException("Cannot detach an object that is not attached to the current context.");
            }
            entry.State = EntityState.Detached;
            return item;
        }

        private DbEntityEntry<T> GetChangeTrackingEntry<T>(T item) where T : class
        {
            var entry = base.Entry(item);
            return entry;
        }

        public T Reload<T>(T item) where T : class
        {
            var entry = GetChangeTrackingEntry(item);
            if(entry == null)
            {
                throw new InvalidOperationException("You cannot reload an objecct that is not in the current Entity Framework datya context");
            }
            entry.Reload();
            return item;
        }

        public void Reload<T>() where T : class
        {
            var entries = base.ChangeTracker.Entries<T>();
            entries.Each(x => x.Reload());
        }

        public int Commit()
        {
            base.ChangeTracker.DetectChanges();
            InvokePreSave();
            var result = base.SaveChanges();
            InvokePostSave();
            return result;
        }

        private void InvokePostSave()
        {
            if (PostSave != null) PostSave(this, new PostSaveEventArgs());
        }

        private void InvokePreSave()
        {
            if (PreSave != null) PreSave(this, new PreSaveEventArgs(){});
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            return base.Database.SqlQuery<T>(sql, dbParams);
        }

        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            return base.Database.ExecuteSqlCommand(sql, dbParams);
        }

        public int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams)
        {
            return base.Database.SqlQuery<int>(procedureName, dbParams).FirstOrDefault();
        }

        private IEventManager _eventManager;
        public IEventManager EventManager
        {
            get { return _eventManager; }
            set 
            { 
                _eventManager = value;
                _eventManager.Context = this;
            }
        }

        public event EventHandler<PreSaveEventArgs> PreSave;
        public event EventHandler<PostSaveEventArgs> PostSave;
    }
}