using System.Collections.Specialized;

namespace Highway.Data
{
    /// <summary>
    /// Pre-Built Query that applies Odata parameters to query on Type
    /// </summary>
    /// <typeparam name="T">The Type to be queried</typeparam>
    public class GetByOData<T> : ODataQuery<T> where T : class
    {
        public GetByOData(NameValueCollection queryParameters)
            : base(queryParameters)
        {
            ContextQuery = c => c.AsQueryable<T>();
        }
    }
}