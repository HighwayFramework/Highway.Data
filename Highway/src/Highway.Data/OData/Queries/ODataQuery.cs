using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.OData;
using Highway.Data.OData.Parser;

namespace Highway.Data
{
    /// <summary>
    /// Query that applies OData query parameters to the expression before Sql Execution
    /// </summary>
    /// <typeparam name="T">The Type under query</typeparam>
    public class ODataQuery<T> : QueryBase, IScalar<ODataResult<T>>
        where T : class
    {
        public ODataQuery(NameValueCollection queryParameters, ExpandBehavior expandBehavior = ExpandBehavior.None)
        {
            queryParameters = ApplyExpansionBehavior(queryParameters, expandBehavior);
            ModelFilter = new ParameterParser<T>().Parse(queryParameters);
        }

        private NameValueCollection ApplyExpansionBehavior(NameValueCollection queryParameters, ExpandBehavior expandBehavior)
        {
            switch (expandBehavior)
            {
                case ExpandBehavior.OneLevel:
                    {
                        var values = queryParameters.GetValues("$expand");
                        if (values != null)
                        {
                            queryParameters.Remove("$expand");
                            foreach (var value in values.Where(value => !value.Contains("/")))
                            {
                                queryParameters.Add("$expand", value);
                            }
                        }
                    }
                    break;
                case ExpandBehavior.None:
                    {
                        queryParameters.Remove("$expand");
                        return queryParameters;
                    }
                default:
                    return queryParameters;

            }
            throw new NotImplementedException();
        }

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a an <see cref="ODataResult{T}"/>
        /// </summary>
        /// <param name="dataContext">the data context that the query should be executed against</param>
        /// <returns>
        ///     <see cref="ODataResult{T}"/>
        /// </returns>
        public virtual ODataResult<T> Execute(IDataContext dataContext)
        {
            var task = PrepareQuery(dataContext);
            return new ODataResult<T>(task);
        }

        public virtual string OutputQuery(IDataContext context)
        {
            IQueryable<object> query = PrepareQuery(context);
            return query.ToString();
        }

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuild Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{T}" /></returns>
        protected virtual IQueryable<T> ExtendQuery()
        {
            try
            {
                return ContextQuery(Context);
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
        protected virtual IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            IQueryable<T> source = query;
            foreach (var exp in ExpressionList)
            {
                List<Expression> newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<T>(Expression.Call(null, exp.Item1, newParams));
            }
            source = source.Filter(ModelFilter);
            return source;
        }

        /// <summary>
        /// Prepares query for execution by appending projection expression
        /// </summary>
        /// <param name="context">The context to execute against</param>
        /// <returns>The prepared but unexecuted query</returns>
        protected virtual UntypedQueryable<T> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            IQueryable<T> query = ExtendQuery();
            var preppedQuery = AppendExpressions(query);
            return ModelFilter.Project(preppedQuery);
        }

        /// <summary>
        /// The query to be later executed against the context
        /// </summary>
        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }

        protected readonly IModelFilter<T> ModelFilter;
    }

    /// <summary>
    /// Expand behavior modifies how the $expand keyword in OData functions
    /// </summary>
    public enum ExpandBehavior
    {
        /// <summary>
        /// None disables the ability to expand at all ( this is the Highway.Data default )
        /// </summary>
        None,
        /// <summary>
        /// SingleLevel allows for expansion of objects directly related to the object under query
        /// </summary>
        OneLevel,
        /// <summary>
        /// All allows for any expansion ( this is the OData specification default )
        /// </summary>
        All
    }
}