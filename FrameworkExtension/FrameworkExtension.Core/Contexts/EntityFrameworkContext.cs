using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Services;

namespace FrameworkExtension.Core.Contexts
{
    public class EntityFrameworkContext : DbContext, IDataContext
    {
        private readonly IUserNameService _userNameService;

        public EntityFrameworkContext(string connectionString) : this(connectionString, new DefaultUserNameService())
        {
        }

        public EntityFrameworkContext(string connectionString, IUserNameService userNameService) : base(connectionString)
        {
            _userNameService = userNameService;
        }


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
            var userName = _userNameService.GetCurrentUserName();
#if DEBUG
            var addedEntities = this.ChangeTracker.Entries().Where(x=>x.State == EntityState.Added).Where(e => e.Entity is IAuditableEntity).ToList();
            var modifiedEntities = this.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Where(e => e.Entity is IAuditableEntity).ToList();
            var deletedEntities = this.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Where(e => e.Entity is IAuditableEntity).ToList();
#endif

            this.ChangeTracker.Entries().Where(x => x.State == EntityState.Added)
                .Where(e => e.Entity is IAuditableEntity)
                .ToList()
                .ForEach(e =>
                {
                    var entity = e.Entity as IAuditableEntity;
                    entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
                    entity.CreatedBy = entity.ModifiedBy = userName;
                });

            this.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified)
                .Where(e => e.Entity is IAuditableEntity)
                .ToList()
                .ForEach(e =>
                {
                    var entity = e.Entity as IAuditableEntity;
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = userName;
                });
            this.ChangeTracker.DetectChanges();
            var result = base.SaveChanges();
            return result;
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
    }
}