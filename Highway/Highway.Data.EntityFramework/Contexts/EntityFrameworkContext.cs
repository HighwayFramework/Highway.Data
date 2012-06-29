using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.Interceptors.Events;
using Highway.Data.Interfaces;
using Highway.Data;

namespace Highway.Data.EntityFramework.Contexts
{
    public class EntityFrameworkContext : DbContext, IObservableDataContext
    {
        private readonly IMappingConfiguration _configuration;

        /// <summary>
        /// Constructs a context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="configuration">The Mapping Configuration that will determine how the tables and objects interact</param>
        public EntityFrameworkContext(string connectionString, IMappingConfiguration configuration) : base(connectionString)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This gives a mockable wrapper around the normal Set<typeparam name="T"></typeparam> method that allows for testablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>IQueryable<typeparam name="T"></typeparam></returns>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.Set<T>();
        }

        /// <summary>
        /// Adds the provided instance of <typeparam name="T"></typeparam> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being added</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to add</param>
        /// <returns>The <typeparam name="T"></typeparam> you added</returns>
        public T Add<T>(T item) where T : class
        {
            this.Set<T>().Add(item);
            return item;
        }

        /// <summary>
        /// Removes the provided instance of <typeparam name="T"></typeparam> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being removed</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to remove</param>
        /// <returns>The <typeparam name="T"></typeparam> you removed</returns>
        public T Remove<T>(T item) where T : class
        {
            this.Set<T>().Remove(item);
            return item;
        }

        /// <summary>
        /// Updates the provided instance of <typeparam name="T"></typeparam> in the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being updated</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to update</param>
        /// <returns>The <typeparam name="T"></typeparam> you updated</returns>
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

        /// <summary>
        /// Attaches the provided instance of <typeparam name="T"></typeparam> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to attach</param>
        /// <returns>The <typeparam name="T"></typeparam> you attached</returns>
        public T Attach<T>(T item) where T : class
        {
            this.Set<T>().Attach(item);
            return item;
        }

        /// <summary>
        /// Detaches the provided instance of <typeparam name="T"></typeparam> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to detach</param>
        /// <returns>The <typeparam name="T"></typeparam> you detached</returns>
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

        /// <summary>
        /// Reloads the provided instance of <typeparam name="T"></typeparam> from the database
        /// </summary>
        /// <typeparam name="T">The Entity Type being reloaded</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to reload</param>
        /// <returns>The <typeparam name="T"></typeparam> you reloaded</returns>
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

        /// <summary>
        /// Reloads all tracked objects of the type <typeparam name="T"></typeparam>
        /// </summary>
        /// <typeparam name="T">The type of objects to reload</typeparam>
        public void Reload<T>() where T : class
        {
            var entries = base.ChangeTracker.Entries<T>();
            entries.Each(x => x.Reload());
        }
        
        /// <summary>
        /// Commits all currently tracked entity changes
        /// </summary>
        /// <returns>the number of rows affected</returns>
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

        /// <summary>
        /// Executes a SQL command and tries to map the returned datasets into an IEnumerable<typeparam name="T"></typeparam>
        /// The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An IEnumerable<typeparam name="T"></typeparam> from the query return</returns>
        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            return base.Database.SqlQuery<T>(sql, dbParams);
        }

        /// <summary>
        /// Executes a SQL command and returns the standard int return from the query
        /// </summary>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            return base.Database.ExecuteSqlCommand(sql, dbParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _configuration.ConfigureModelBuilder(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}