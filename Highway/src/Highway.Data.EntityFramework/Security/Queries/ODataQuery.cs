using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;
using Highway.Data.OData;
using Highway.Data.OData.Parser;

namespace Highway.Data.EntityFramework.Security.Queries
{
    public class ODataQuery<T> : QueryBase, IQuery<T, object>
        where T : class
    {
        private readonly IModelFilter<T> _modelFilter;

        public ODataQuery(NameValueCollection queryParameters)
        {
            _modelFilter = new ParameterParser<T>().Parse(queryParameters);
        }

        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }

        public virtual IEnumerable<object> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            return PrepareQuery(context).ToString();
        }

        public string OutputSQLStatement(IDataContext context)
        {
            return OutputQuery(context);
        }

        protected IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            var queryable = query;
            foreach (var tuple in ExpressionList)
            {
                var list = tuple.Item2.ToList();
                list.Insert(0, queryable.Expression);
                queryable = queryable.Provider.CreateQuery<T>(Expression.Call(null, tuple.Item1, list));
            }
            queryable = AppendSecurity(queryable);
            return queryable;
        }

        protected virtual IQueryable<T> ExtendQuery()
        {
            return ContextQuery(Context);
        }

        private IQueryable<T> AppendSecurity(IQueryable<T> queryable)
        {
            var expression = queryable.Expression;
            var estreamSecurityExpressionVistor = new EstreamSecurityExpressionVistor();
            estreamSecurityExpressionVistor.Visit(expression);
            var metadatas = estreamSecurityExpressionVistor.Metadatas;
            var typedContext = (ISecuredDataContext) Context;
            foreach (var metadata in metadatas)
            {
                var predicate = metadata.GenerateSecurityPredicate<T>(typedContext.EntitlementProvider,
                    typedContext.SecuredRelationshipCache);
                if (predicate != null)
                {
                    queryable = queryable.Where(predicate);
                }
            }
            return queryable.Filter(_modelFilter);
        }

        private IEnumerable<object> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            var preppedQuery = AppendExpressions(ExtendQuery());
            return _modelFilter.Project(preppedQuery);
        }
    }
}