using System.Collections.Generic;
using System.Threading.Tasks;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
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
}