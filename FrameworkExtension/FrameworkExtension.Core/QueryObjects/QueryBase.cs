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
    public abstract class QueryBase : IExtendableQuery
    {
        static ReadOnlyCollection<MethodInfo> QueryableMethods;
        static QueryBase()
        {
            QueryableMethods = new ReadOnlyCollection<MethodInfo>(typeof(System.Linq.Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static).ToList());
        }

        //if this func returns IQueryable then we can add functionaltly like 
        //Where, OrderBy, Take, etc to the QueryOjbect and inject that into the 
        //expression before is it is executed


        protected List<Tuple<MethodInfo, Expression[]>> _expressionList = new List<Tuple<MethodInfo, Expression[]>>();

        public void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters)
        {
            MethodInfo orderMethodInfo = QueryableMethods
                .Where(m => m.Name == methodName && m.GetParameters().Length == parameters.Length + 1).First();

            orderMethodInfo = orderMethodInfo.MakeGenericMethod(generics);
            _expressionList.Add(new Tuple<MethodInfo, Expression[]>(orderMethodInfo, parameters));
        }


        protected IDataContext Context { get; set; }

        protected void CheckContextAndQuery(object query)
        {
            if (Context == null) throw new InvalidOperationException("Context cannot be null.");
            if (query == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }

    }
}
