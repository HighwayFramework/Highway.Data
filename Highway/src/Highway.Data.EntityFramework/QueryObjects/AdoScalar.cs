using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalar<T> : AdoScalarBase, IScalar<T>
    {
        public abstract string Query { get; }

        public T Execute(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            Func<DbContext, T> contextQuery = c =>
            {
                var cmd = c.CreateSqlCommand(Query, Parameters?.ToArray());
                return cmd.ExecuteCommandWithResult(MapReaderResults);
            };
            return contextQuery(efContext);
        }

        protected abstract T MapReaderResults(IDataReader reader);
    }
}
