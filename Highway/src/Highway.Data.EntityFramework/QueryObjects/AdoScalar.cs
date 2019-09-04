using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalar<T> : QueryBase, IScalar<T>
    {
        private Func<DbContext, T> _contextQuery;

        protected T Output { get; set; }

        protected IEnumerable<IDataParameter> Parameters { get; set; }

        protected abstract string Query { get; }

        protected DbContext TypedContext { get; set; }

        public T Execute(IDataContext context)
        {
            TypedContext = (DbContext)context;
            Parameters = GetParameters();
            _contextQuery = c =>
            {
                var cmd = c.CreateSqlCommand(Query, Parameters?.ToArray());
                Output = cmd.ExecuteCommandWithResult(MapReaderResults);
                return Output;
            };
            return _contextQuery(TypedContext);
        }

        protected abstract IEnumerable<IDataParameter> GetParameters();

        protected abstract T MapReaderResults(IDataReader reader);
    }
}
