using Highway.Data.Interceptors.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data
{
    /// <summary>
    ///     The base query that allows for appending expressions, query extensions, and eventing
    /// </summary>
    public abstract class QueryBase : IExtendableQuery, IObservableQuery
    {
        private static readonly ReadOnlyCollection<MethodInfo> QueryableMethods;

        /// <summary>
        ///     Holds the expressions to be appended
        /// </summary>
        protected List<Tuple<MethodInfo, Expression[]>> ExpressionList = new List<Tuple<MethodInfo, Expression[]>>();

        static QueryBase()
        {
            QueryableMethods = new ReadOnlyCollection<MethodInfo>(typeof(Queryable)
                .GetMethods(BindingFlags.Public |
                            BindingFlags.Static).ToList());
        }

        /// <summary>
        ///     The reference to the <see cref="IReadonlyDataContext" /> that gives data connection
        /// </summary>
        protected IReadonlyDataContext Context { get; set; }


        /// <summary>
        ///     Adds a method to the expression in the query object
        /// </summary>
        /// <param name="methodName">The name of the method to be added i.e. GroupBy</param>
        /// <param name="generics">Any type parameters needed by the method to be added</param>
        /// <param name="parameters">Any object parameters needed by the method to be added</param>
        public virtual void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters)
        {
            MethodInfo orderMethodInfo =
                QueryableMethods.First(m => m.Name == methodName && m.GetParameters().Length == parameters.Length + 1);

            orderMethodInfo = orderMethodInfo.MakeGenericMethod(generics);
            ExpressionList.Add(new Tuple<MethodInfo, Expression[]>(orderMethodInfo, parameters));
        }



        /// <summary>
        ///     The event fired just before the query goes to the database
        /// </summary>
        public event EventHandler<BeforeQuery> BeforeQuery;

        /// <summary>
        ///     The event fire just after the data is translated into objects but before the data is returned.
        /// </summary>
        public event EventHandler<AfterQuery> AfterQuery;


        /// <summary>
        ///     Checks the context and the Query for null
        /// </summary>
        /// <param name="query">The query to be executed</param>
        protected virtual void CheckContextAndQuery(object query)
        {
            if (Context == null) throw new InvalidOperationException("DataContext cannot be null.");
            if (query == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }
    }
}