using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

namespace Highway.Data
{
    public abstract class AdoBase : QueryBase
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        protected abstract string SqlStatement { get; }

        protected DbContext GetTypedContext(IDataContext context)
        {
            var efContext = context as DbContext;
            if (efContext == null)
            {
                throw new InvalidOperationException("You cannot execute EF Sql Queries against a non-EF context");
            }

            return efContext;
        }
    }
}