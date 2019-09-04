using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalar<T> : AdoScalarBase<T>
    {
        protected abstract string Query { get; }

        protected override IDbCommand GetDbCommand(DbContext dbContext)
        {
            return dbContext.CreateAdoCommand(Query, Parameters?.ToArray());
        }
    }
}
