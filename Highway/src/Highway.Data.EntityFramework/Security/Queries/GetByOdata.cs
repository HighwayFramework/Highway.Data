using System.Collections.Specialized;

namespace Highway.Data.EntityFramework.Security.Queries
{
    public class GetByOdata<T> : ODataQuery<T>
        where T : class
    {
        public GetByOdata(NameValueCollection queryParameters) : base(queryParameters)
        {
            ContextQuery = c => c.AsQueryable<T>();
        }
    }
}