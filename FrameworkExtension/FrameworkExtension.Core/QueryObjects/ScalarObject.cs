using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public class ScalarObject<T> : IScalarObject<T> where T : struct
    {
        public Func<IDataContext, T> ContextQuery { get; set; }

        protected void CheckContextAndQuery(IDataContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (this.ContextQuery == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }
        
        public virtual T Execute(IDataContext context)
        {
                CheckContextAndQuery(context);
                return this.ContextQuery(context);
        }
    }    
}
