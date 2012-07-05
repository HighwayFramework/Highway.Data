using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// Base implementation of a query that returns a single value or object
    /// </summary>
    /// <typeparam name="T">The type of object or value being returned</typeparam>
    public class Scalar<T> : QueryBase, IScalarObject<T>
    {
        protected Func<IDataContext, T> ContextQuery { get; set; }

        public T Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            return this.ContextQuery(context);
        }
    }    
}
