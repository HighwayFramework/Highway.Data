#region

using System.Linq;

#endregion

namespace Highway.Data.Rest.Configuration.Interfaces
{
    public interface IHttpClientAdapter
    {
        IQueryable<T> All<T>();
    }
}