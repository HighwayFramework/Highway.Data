using System.Collections.Generic;
using System.Data;

namespace Highway.Data
{
    public abstract class AdoCommandBase : AdvancedCommand
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }
    }
}