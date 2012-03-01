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
    public class QueryObject<T> : QueryObjectBase<T>
    {
        //protected override IQueryable<T> ExtendQuery()
        //{
        //    var source = base.ExtendQuery();
        //    source = this.AppendExpressions(source);
        //    return source;
        //}


        //private IQueryable<T> AppendExpressions(IQueryable<T> query)
        //{
        //    var source = query;
        //    foreach (var exp in _expressionList)
        //    {
        //        var newParams = exp.Item2.ToList();
        //        newParams.Insert(0, source.Expression);
        //        source = source.Provider.CreateQuery<T>(Expression.Call(null, exp.Item1, newParams));
        //    }
        //    return source;
        //}

    }
}
