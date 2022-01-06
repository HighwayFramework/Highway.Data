using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace Highway.Data
{
    public abstract class ReadonlyDbContext : DbContext
    {
        protected ReadonlyDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected ReadonlyDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException($"Do not call {nameof(SaveChanges)} on a {nameof(ReadonlyDbContext)}.");
        }

        public override Task<int> SaveChangesAsync()
        {
            throw new InvalidOperationException($"{nameof(ReadonlyDataContext)} does not implement {nameof(SaveChangesAsync)}");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new InvalidOperationException($"{nameof(ReadonlyDataContext)} does not implement {nameof(SaveChangesAsync)}");
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            throw new InvalidOperationException($"{nameof(ReadonlyDataContext)} does not implement {nameof(Set)}");
        }

        public override DbSet Set(Type entityType)
        {
            throw new InvalidOperationException($"{nameof(ReadonlyDataContext)} does not implement {nameof(Set)}");
        }

        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            throw new InvalidOperationException($"{nameof(ReadonlyDataContext)} does not implement {nameof(ShouldValidateEntity)}");
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            throw new InvalidOperationException($"{nameof(ReadonlyDataContext)} does not implement {nameof(ValidateEntity)}");
        }
    }
}
