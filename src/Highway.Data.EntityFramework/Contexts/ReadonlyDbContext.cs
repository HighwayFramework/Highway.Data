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

        public sealed override int SaveChanges()
        {
            throw new InvalidOperationException($"Do not call {nameof(SaveChanges)} on a {nameof(ReadonlyDbContext)}.");
        }

        public sealed override Task<int> SaveChangesAsync()
        {
            throw new InvalidOperationException($"Do not call {nameof(SaveChangesAsync)} on a {nameof(ReadonlyDbContext)}.");
        }

        public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new InvalidOperationException($"Do not call {nameof(SaveChangesAsync)} on a {nameof(ReadonlyDbContext)}.");
        }

        protected sealed override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            throw new InvalidOperationException($"Do not call {nameof(ShouldValidateEntity)} on a {nameof(ReadonlyDbContext)}.");
        }

        protected sealed override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            throw new InvalidOperationException($"Do not call {nameof(ValidateEntity)} on a {nameof(ReadonlyDbContext)}.");
        }
    }
}
