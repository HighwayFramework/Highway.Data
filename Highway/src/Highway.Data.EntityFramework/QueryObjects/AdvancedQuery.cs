#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     This query is tied to the implementation of EntityFramework
    /// </summary>
    public class AdvancedQuery<T> : QueryBase, IQuery<T>
    {
        /// <summary>
        ///     This holds the expression that will be used to create the <see cref="IQueryable{T}" /> when executed on the context
        /// </summary>
        protected Func<DataContext, IQueryable<T>> ContextQuery { get; set; }

        #region IQuery<T> Members

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            IQueryable<T> task = PrepareQuery(context);
            return task;
        }

        /// <summary>
        ///     This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the
        ///     IQueryable<typeparamref name="T" /> against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        public string OutputSQLStatement(IDataContext context)
        {
            return OutputQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            IQueryable<T> query = PrepareQuery(context);
            return query.ToString();
        }

        #endregion

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuild Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{T}" /></returns>
        protected virtual IQueryable<T> ExtendQuery()
        {
            try
            {
                return ContextQuery((DataContext) Context);
            }
            catch (Exception)
            {
                throw; //just here to catch while debugging
            }
        }

        /// <summary>
        ///     Gives the ability to apend an <see cref="IQueryable" /> onto the current query
        /// </summary>
        /// <param name="query">The query containing the expressions to append</param>
        /// <returns>The combined query</returns>
        protected IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            IQueryable<T> source = query;
            foreach (var exp in ExpressionList)
            {
                List<Expression> newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<T>(Expression.Call(null, exp.Item1, newParams));
            }
            return source;
        }

        private IQueryable<T> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            IQueryable<T> query = ExtendQuery();
            return AppendExpressions(query);
        }
    }

    /// <summary>
    ///     The base implemetation of a query that has a projection
    /// </summary>
    /// <typeparam name="TSelection">The Type that will be selected</typeparam>
    /// <typeparam name="TProjection">The type that will be projected</typeparam>
    public class AdvancedQuery<TSelection, TProjection> : QueryBase, IQuery<TSelection, TProjection>
        where TSelection : class
    {
        protected Func<DataContext, IQueryable<TSelection>> Selector { get; set; }
        protected Func<IQueryable<TSelection>, IQueryable<TProjection>> Projector { get; set; }

        #region IQuery<T> Members

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public virtual IEnumerable<TProjection> Execute(IDataContext context)
        {
            IQueryable<TProjection> task = PrepareQuery(context);
            return task;
        }

        /// <summary>
        ///     This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the
        ///     IQueryable<typeparamref name="T" /> against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        public string OutputSQLStatement(IDataContext context)
        {
            return OutputQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            IQueryable<TProjection> query = PrepareQuery(context);

            return query.ToString();
        }

        #endregion

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuild Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{TSelection}" /></returns>
        protected virtual IQueryable<TSelection> ExtendQuery()
        {
            return Selector((DataContext) Context);
        }

        /// <summary>
        ///     Gives the ability to apend an <see cref="IQueryable" /> onto the current query
        /// </summary>
        /// <param name="query">The query containing the expressions to append</param>
        /// <returns>The combined query</returns>
        protected IQueryable<TProjection> AppendExpressions(IQueryable<TSelection> query)
        {
            var source = query;
            foreach (var exp in ExpressionList)
            {
                List<Expression> newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<TSelection>(Expression.Call(null, exp.Item1, newParams));
            }
            return Projector(source);
        }

        private IQueryable<TProjection> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(Selector);
            var query = ExtendQuery();
            return AppendExpressions(query);
        }
    }
}