﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalar<T> : AdoBase, IScalar<T>
    {
        public abstract string Query { get; }

        public T Execute(IDataContext context)
        {
            var efContext = GetTypedContext(context);
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
