using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalar<T> : AdoScalarBase<T>, IScalar<T>
    {
        public abstract string Query { get; }

        protected override IDbCommand GetCommand(DbContext c)
        {
            return c.CreateAdoCommand(Query, Parameters?.ToArray());
        }
    }
}
