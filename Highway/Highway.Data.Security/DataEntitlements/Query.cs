using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Security
{
    public class Query<T> : QueryBase, IQuery<T>
        where T : class
    {
        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            return PrepareQuery(context).ToString();
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
            var securityExpressionVistor = new SecurityExpressionVistor();
            securityExpressionVistor.Visit(expression);
            var metadatas = securityExpressionVistor.Metadatas;
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

        private IQueryable<T> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            return AppendExpressions(ExtendQuery());
        }

        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }
    }
}
