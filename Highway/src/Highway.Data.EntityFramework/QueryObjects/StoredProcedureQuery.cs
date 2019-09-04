using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureQuery<T> : AdoQueryBase<T>, IQuery<T>
    {
        protected abstract string StoredProcedureName { get; }

        private IDbCommand GetCommand(DbContext c)
        {
            return c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
        }
    }
}
