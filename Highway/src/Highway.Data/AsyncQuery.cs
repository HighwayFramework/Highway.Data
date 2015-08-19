using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data
{
    public class AsyncQuery<T> : IAsyncQuery<T>
    {
        protected Func<IDataContext, Task<object>> ContextQuery;

        public Task<IEnumerable<T>> Execute(IDataContext context)
        {
            return ContextQuery(context).ContinueWith<IEnumerable<T>>(t => t.Result as IEnumerable<T>);
        }
    }

    public interface IAsyncQuery<T>
    {
        Task<IEnumerable<T>> Execute(IDataContext context);
    }
}
