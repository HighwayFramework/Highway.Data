using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using FrameworkExtension.Core.Interfaces;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;

namespace FrameworkExtension.Core.QueryObjects
{
    public static class QueryExtensions
    {
        public static IQueryObject<T> Take<T>(this IQueryObject<T> extend, int count)
        {
            var generics = new Type[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Take", generics, parameters);
            return extend;
        }

        public static IQueryObject<T> Skip<T>(this IQueryObject<T> extend, int count)
        {
            var generics = new Type[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Skip", generics, parameters);
            return extend;
        }
    }

    public abstract class QueryObjectBase<T> : IQueryObject<T>, IExtendableQuery
    {

        static ReadOnlyCollection<MethodInfo> QueryableMethods;
        static QueryObjectBase()
        {
            QueryableMethods = new ReadOnlyCollection<MethodInfo>(typeof(System.Linq.Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static).ToList());
        }

        //if this func returns IQueryable then we can add functionaltly like 
        //Where, OrderBy, Take, etc to the QueryOjbect and inject that into the 
        //expression before is it is executed


        List<Tuple<MethodInfo, Expression[]>> _expressionList = new List<Tuple<MethodInfo, Expression[]>>();
        private void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters)
        {
            MethodInfo orderMethodInfo = QueryableMethods
                .Where(m => m.Name == methodName && m.GetParameters().Length == parameters.Length + 1).First();

            orderMethodInfo = orderMethodInfo.MakeGenericMethod(generics);
            _expressionList.Add(new Tuple<MethodInfo, Expression[]>(orderMethodInfo, parameters));
        }


        protected Func<IDbContext, IQueryable<T>> ContextQuery { get; set; }
        protected IDbContext Context { get; set; }

        protected void CheckContextAndQuery()
        {
            if (Context == null) throw new InvalidOperationException("Context cannot be null.");
            if (this.ContextQuery == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }

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

        public virtual IEnumerable<T> Execute(IDbContext context)
        {
            Context = context;
            CheckContextAndQuery();
            var query = this.ExtendQuery();
            query = this.AppendExpressions(query);
            return query;
        }


        private IQueryable<T> AppendExpressions(IQueryable<T> query)
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

        void IExtendableQuery.AddMethodExpression(string methodName, Type[] generics, Expression[] parameters)
        {
            MethodInfo orderMethodInfo = QueryableMethods
                .Where(m => m.Name == methodName && m.GetParameters().Length == parameters.Length + 1).First();

            orderMethodInfo = orderMethodInfo.MakeGenericMethod(generics);
            
            _expressionList.Add(new Tuple<MethodInfo, Expression[]>(orderMethodInfo, parameters));
        }
    }
}
