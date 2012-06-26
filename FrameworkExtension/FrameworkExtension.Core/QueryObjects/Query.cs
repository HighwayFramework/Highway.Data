using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq.Expressions;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public class Query<T> : QueryBase, IQuery<T>
    {
        /// <summary>
        /// This holds the expression that will be used to create the IQueryable<typeparam name="T"></typeparam> when executed on the context
        /// </summary>
        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }

        /// <summary>
        /// This method allows for the extension of Ordering and Grouping on the prebuild Query
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<T> ExtendQuery()
        {
            try
            {
                return this.ContextQuery(Context);
            }
            catch (Exception)
            {
                throw; //just here to catch while debugging
            }
        }

        #region IQueryObject<T> Members

        /// <summary>
        /// This executes the expression in ContextQuery on the context that is passed in, resulting in a IQueryable<typeparam name="T"></typeparam> that is returned as an IEnumerable<typeparam name="T"></typeparam>
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>IEnumerable<typeparam name="T"></typeparam></returns>
        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            var query = PrepareQuery(context);
            return query;
        }

        protected IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            var source = query;
            foreach (var exp in _expressionList)
            {
                var newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<T>(Expression.Call(null, exp.Item1, newParams));
            }
            return source;
        }

        /// <summary>
        /// This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the IQueryable<typeparam name="T"></typeparam> against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        public string OutputSQLStatement(IDataContext context)
        {
            var query = PrepareQuery(context);
            return query.ToString();
        }

        private IQueryable<T> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            var query = this.ExtendQuery();
            return this.AppendExpressions(query);
        }
       

        #endregion
    }
}
