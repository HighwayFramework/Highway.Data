using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Highway.Data.OData;
using Highway.Data.OData.Parser;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Security.DataEntitlements
{
    public class SecuredODataQuery<T> : QueryBase, IQuery<T, object>
        where T : class
    {
        public SecuredODataQuery(NameValueCollection queryParameters)
        {
            _modelFilter = new ParameterParser<T>().Parse(queryParameters);
        }

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
            IQueryable<T> queryable = query;
            foreach (Tuple<MethodInfo, Expression[]> tuple in ExpressionList)
            {
                List<Expression> list = tuple.Item2.ToList();
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
            var SecurityExpressionVistor = new SecurityExpressionVistor();
            SecurityExpressionVistor.Visit(expression);
            var metadatas = SecurityExpressionVistor.Metadatas;
            var typedContext = (ISecuredDataContext)Context;
            foreach (var metadata in metadatas)
            {
                var predicate = metadata.GenerateSecurityPredicate<T>(typedContext.EntitlementProvider, typedContext.MappingsCache);
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
            if (_modelFilter.IncludeCount)
            {
                return new UntypedCountedQueryable<T>(preppedQuery, _modelFilter.ProjectExpression, preppedQuery.Count());    
            }
            return new UntypedQueryable<T>(preppedQuery, _modelFilter.ProjectExpression);
        }

        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }

        private readonly IModelFilter<T> _modelFilter;
    }
}
