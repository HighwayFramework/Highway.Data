using System.Collections.Generic;
using System.Data;

namespace Highway.Data
{
    public abstract class AdoScalarBase : QueryBase
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }
    }
}