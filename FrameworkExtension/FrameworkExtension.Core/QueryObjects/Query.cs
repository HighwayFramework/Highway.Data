using System;
using System.Collections.Generic;
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
        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }

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

        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            var query = this.ExtendQuery();
            query = this.AppendExpressions(query);
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
        #endregion
    }
}
