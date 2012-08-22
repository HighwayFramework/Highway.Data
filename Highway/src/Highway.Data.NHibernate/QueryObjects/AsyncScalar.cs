using System.Threading.Tasks;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    public class AsyncScalar<T> : IAsyncScalar<T>
    {
        private readonly IScalar<T> _query;

        public AsyncScalar(IScalar<T> query)
        {
            _query = query;
        }


        public Task<T> Execute(IDataContext context)
        {
            return new Task<T>(() =>
                {
                    lock (context)
                    {
                        return _query.Execute(context);    
                    }
                });
        }
    }
}