using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureScalar<T> : AdoBase, IScalar<T>
    {
        public abstract string StoredProcedureName { get; }

        public T Execute(IDataContext context)
        {
            var efContext = GetTypedContext(context);
            Func<DbContext, T> contextQuery = c =>
            {
                var cmd = c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
                return cmd.ExecuteCommandWithResult(MapReaderResults);
            };
            return contextQuery(efContext);
        }

        protected abstract T MapReaderResults(IDataReader reader);
    }
}
