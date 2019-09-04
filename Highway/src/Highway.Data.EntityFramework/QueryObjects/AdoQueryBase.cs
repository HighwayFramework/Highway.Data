using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.Extensions;

namespace Highway.Data
{
    // ERIC:  Derive from this?  Seems heavy-handed.
    public abstract class AdoQueryBase<T> : QueryBase
    {
        protected Func<DbContext, IQueryable<T>> ContextQuery { get; }

        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        public AdoQueryBase()
        {
            ContextQuery = dbContext =>
            {
                var dbCommand = GetDbCommand(dbContext);
                return dbCommand.ExecuteWithResults(MapReaderResults);
            };
        }

        public virtual IEnumerable<T> Execute(IDataContext dataContext)
        {
            return PrepareQuery(dataContext);
        }

        // ERIC:  Do we want this?  Implementation?
        public string OutputQuery(IDataContext dataContext)
        {
            var entityDbContext = dataContext.GetEntityDbContext();
            var dbCommand = GetDbCommand(entityDbContext);
            return dbCommand.CommandText;
        }

        protected virtual IQueryable<T> PrepareQuery(IDataContext context)
        {
            var entityDbContext = context.GetEntityDbContext();
            return ContextQuery(entityDbContext);
        }

        protected abstract IDbCommand GetDbCommand(DbContext dbContext);

        protected abstract IEnumerable<T> MapReaderResults(IDataReader dataReader);
    }
}
