using System.Collections.Generic;
using System.Threading.Tasks;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncQuery<T> : IAsyncQuery<T>
    {
        private readonly IQuery<T> _query;

        /// <summary>
        /// Constructs an AsyncQuery from an existing non-Async Query
        /// </summary>
        /// <param name="query"></param>
        public AsyncQuery(IQuery<T> query)
        {
            _query = query;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Execute(IDataContext context)
        {

            return new Task<IEnumerable<T>>(() =>
                {
                    lock (context)
                    {
                        return _query.Execute(context);
                    }
                });


        }

        public string OutputSQLStatement(IDataContext context)
        {
            return _query.OutputSQLStatement(context);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AsyncScalar<T> : IAsyncScalar<T>
    {
        private readonly IScalar<T> _query;

        public AsyncScalar(IScalar<T> query)
        {
            _query = query;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<T> Execute(IDataContext context)
        {
            return new Task<T>(() => _query.Execute(context));
        }
    }
}