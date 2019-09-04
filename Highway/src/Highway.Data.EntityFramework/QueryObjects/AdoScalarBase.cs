using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data
{
    // ERIC:  Derive from this?  Seems heavy-handed.
    public abstract class AdoScalarBase<T> : IScalar<T>
    {
        // ERIC:  Discuss
        protected Func<DbContext, T> ContextQuery { get; }

        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        protected AdoScalarBase()
        {
            ContextQuery = dbContext =>
            {
                var dbCommand = GetDbCommand(dbContext);
                return dbCommand.ExecuteWithResult(MapReaderResults);
            };
        }

        public T Execute(IDataContext dataContext)
        {
            var entityDbContext = dataContext.GetEntityDbContext();
            return ContextQuery(entityDbContext);
        }

        protected abstract IDbCommand GetDbCommand(DbContext dbContext);

        protected abstract T MapReaderResults(IDataReader dataReader);
    }
}
