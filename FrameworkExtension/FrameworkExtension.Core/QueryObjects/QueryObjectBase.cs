using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public abstract class QueryObjectBase<T> : IQueryObject<T>
    {
        //if this func returns IQueryable then we can add functionaltly like 
        //Where, OrderBy, Take, etc to the QueryOjbect and inject that into the 
        //expression before is it is executed
        protected Func<IDbContext, IQueryable<T>> ContextQuery { get; set; }
        protected IDbContext Context { get; set; }

        protected void CheckContextAndQuery()
        {
            if (Context == null) throw new InvalidOperationException("Context cannot be null.");
            if (this.ContextQuery == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }

        protected virtual IQueryable<T> ExtendQuery()
        {
            try
            {
                return this.ContextQuery(Context);
            }
            catch (Exception)
            {                
                throw; //just here to catch while debugging
            }
        }

        #region IQueryObject<T> Members

        public virtual IEnumerable<T> Execute(IDbContext context)
        {
            Context = context;
            CheckContextAndQuery();
            var query = this.ExtendQuery();
            return query;    
        }

        #endregion
    }
}
