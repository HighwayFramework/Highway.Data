using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Data
{
    public interface IAsyncQuery<T>
    {
        Task<IEnumerable<T>> Execute(IDataContext context);
    }
}