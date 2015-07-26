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
        protected Expression<Func<IDataContext, Task>> ContextQuery;

        public Task<IEnumerable<T>> Execute(IDataContext context)
        {
            throw new NotImplementedException();
        }
    }

    public interface IAsyncQuery<T>
    {
        Task<IEnumerable<T>> Execute(IDataContext context);
    }
}
