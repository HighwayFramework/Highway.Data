using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Data
{
    public class ReadonlyRepository : IReadonlyRepository
    {
        public IReadonlyDataContext Context { get; }

        public T Find<T>(IScalar<T> scalar)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find<T>(IQuery<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync<T>(IScalar<T> scalar)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
            where TSelection : class
        {
            throw new NotImplementedException();
        }
    }
}
