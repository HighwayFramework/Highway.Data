using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public class Scalar<T> : QueryBase, IScalarObject<T>
    {
        public Func<IDataContext, T> ContextQuery { get; set; }

        public virtual T Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            return this.ContextQuery(context);
        }
    }    
}
