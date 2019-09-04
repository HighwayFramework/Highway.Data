using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureQuery<T> : QueryBase, IQuery<T>
    {
        private Func<DbContext, IQueryable<T>> _contextQuery;

        protected IQueryable<T> Output { get; set; }

        protected IEnumerable<IDataParameter> Parameters { get; set; }

        protected abstract string StoredProcedureName { get; }

        protected DbContext TypedContext { get; set; }

        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            var c = (DbContext)context;
            var cmd = GetCommand(c);
            return cmd.CommandText;
        }

        protected virtual IQueryable<T> ExtendQuery()
        {
            return _contextQuery((DbContext)Context);
        }

        protected abstract IEnumerable<IDataParameter> GetParameters();

        protected abstract IEnumerable<T> MapReaderResults(IDataReader reader);

        protected virtual IQueryable<T> PrepareQuery(IDataContext context)
        {
            TypedContext = (DbContext)context;
            Parameters = GetParameters();
            _contextQuery = c =>
            {
                var cmd = GetCommand(c);
                Output = cmd.ExecuteCommandWithResults(MapReaderResults);
                return Output;
            };

            return _contextQuery(TypedContext);
        }

        private IDbCommand GetCommand(DbContext c)
        {
            return c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
        }
    }
}
