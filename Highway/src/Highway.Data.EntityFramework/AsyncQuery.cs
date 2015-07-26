using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.Data.EntityFramework
{
    public class AsyncQuery<T> : IAsyncQuery<T>
    {
        protected Func<IDataContext, IQueryable<T>> ContextQuery;

        public Task<IEnumerable<T>> Execute(IDataContext context)
        {
           return ContextQuery(context)
               .ToListAsync()
               .ContinueWith(x => x.Result as IEnumerable<T>);
        }
    }
}
