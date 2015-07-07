using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Highway.Data.OData;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Security
{
    /// <summary>
    /// A Query that applies Odata parameters to expression and also applies record level security
    /// </summary>
    /// <typeparam name="T">The type under query</typeparam>
    public class ODataQuery<T> : Data.ODataQuery<T> where T : class
    {
        public ODataQuery(NameValueCollection queryParameters) : base(queryParameters)
        {
        }
        
        protected override IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            IQueryable<T> queryable = base.AppendExpressions(query);
            queryable = AppendSecurity(queryable);
            return queryable;
        }
    
        private IQueryable<T> AppendSecurity(IQueryable<T> queryable)
        {
            var expression = queryable.Expression;
            var SecurityExpressionVistor = new SecurityExpressionVistor();
            SecurityExpressionVistor.Visit(expression);
            var metadatas = SecurityExpressionVistor.Metadatas;
            var typedContext = (ISecurityContext)Context;
            foreach (var metadata in metadatas)
            {
                var predicate = metadata.GenerateSecurityPredicate<T>(typedContext.EntitlementProvider, typedContext.MappingsCache);
                if (predicate != null)
                {
                    queryable = queryable.Where(predicate);
                }
            }
            return queryable;
        }
    }
}
