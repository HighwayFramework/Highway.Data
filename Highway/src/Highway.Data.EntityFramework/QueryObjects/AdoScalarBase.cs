using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data
{
    // ERIC:  Derive from this?  Seems heavy-handed.
    public abstract class AdoScalarBase<T> : QueryBase
    {
        protected Func<DbContext, T> ContextQuery { get; }

        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        public AdoScalarBase()
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

        // ERIC:  Do we want this?  Implementation?
        public string OutputQuery(IDataContext dataContext)
        {
            var entityDbContext = dataContext.GetEntityDbContext();
            var dbCommand = GetDbCommand(entityDbContext);
            return dbCommand.CommandText;
        }

        protected abstract IDbCommand GetDbCommand(DbContext dbContext);

        protected abstract T MapReaderResults(IDataReader dataReader);
    }
}
