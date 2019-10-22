
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;


// ReSharper disable CheckNamespace

namespace System.Linq.Dynamic
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Microsoft provided class. It allows dynamic string based querying.
    ///     Very handy when, at compile time, you don't know the type of queries that will be generated.
    /// </summary>
    public static class DynamicEnumerable
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, string predicate, params object[] values)
        {
            return (IEnumerable<T>)Where((IEnumerable)source, predicate, values);
        }

        public static IEnumerable Where(this IEnumerable querySource, string predicate, params object[] values)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            IQueryable source = querySource.AsQueryable();
            LambdaExpression lambda = DynamicExpression.ParseLambda(source.ElementType, typeof(bool), predicate, values);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Where",
                    new[] { source.ElementType },
                    source.Expression, Expression.Quote(lambda)));
        }

        public static IEnumerable Select(this IEnumerable querySource, string selector, params object[] values)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            IQueryable source = querySource.AsQueryable();
            LambdaExpression lambda = DynamicExpression.ParseLambda(source.ElementType, null, selector, values);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Select",
                    new[] { source.ElementType, lambda.Body.Type },
                    source.Expression, Expression.Quote(lambda)));
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string ordering, params object[] values)
        {
            return (IQueryable<T>)OrderBy((IQueryable)source, ordering, values);
        }

        public static IEnumerable OrderBy(this IEnumerable querySource, string ordering, params object[] values)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            if (ordering == null) throw new ArgumentNullException("ordering");
            IQueryable source = querySource.AsQueryable();
            var parameters = new[]
            {
                Expression.Parameter(source.ElementType, "")
            };
            var parser = new ExpressionParser(parameters, ordering, values);
            IEnumerable<DynamicOrdering> orderings = parser.ParseOrdering();
            Expression queryExpr = source.Expression;
            string methodAsc = "OrderBy";
            string methodDesc = "OrderByDescending";
            foreach (DynamicOrdering o in orderings)
            {
                queryExpr = Expression.Call(
                    typeof(Queryable), o.Ascending ? methodAsc : methodDesc,
                    new[] { source.ElementType, o.Selector.Type },
                    queryExpr, Expression.Quote(Expression.Lambda(o.Selector, parameters)));
                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }
            return source.Provider.CreateQuery(queryExpr);
        }

        public static IEnumerable Take(this IEnumerable querySource, int count)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            IQueryable source = querySource.AsQueryable();
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Take",
                    new[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        public static IEnumerable Skip(this IEnumerable querySource, int count)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            IQueryable source = querySource.AsQueryable();
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Skip",
                    new[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        public static IEnumerable GroupBy(this IEnumerable querySource, string keySelector, string elementSelector,
            params object[] values)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");
            IQueryable source = querySource.AsQueryable();
            LambdaExpression keyLambda = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            LambdaExpression elementLambda = DynamicExpression.ParseLambda(source.ElementType, null, elementSelector,
                values);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "GroupBy",
                    new[] { source.ElementType, keyLambda.Body.Type, elementLambda.Body.Type },
                    source.Expression, Expression.Quote(keyLambda), Expression.Quote(elementLambda)));
        }

        public static bool Any(this IEnumerable querySource)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            IQueryable source = querySource.AsQueryable();
            return (bool)source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "Any",
                    new[] { source.ElementType }, source.Expression));
        }

        public static int Count(this IEnumerable querySource)
        {
            if (querySource == null) throw new ArgumentNullException("source");
            IQueryable source = querySource.AsQueryable();
            return (int)source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "Count",
                    new[] { source.ElementType }, source.Expression));
        }
    }
}