using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureScalar<T> : AdoScalarBase<T>, IScalar<T>
    {
        public abstract string StoredProcedureName { get; }

        protected override IDbCommand GetCommand(DbContext c)
        {
            return c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
        }
    }
}
