using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureQuery<T> : AdoQueryBase<T>
    {
        protected abstract string StoredProcedureName { get; }

        protected override IDbCommand GetDbCommand(DbContext dbContext)
        {
            return dbContext.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
        }
    }
}
