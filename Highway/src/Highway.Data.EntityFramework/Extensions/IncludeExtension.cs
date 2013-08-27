using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable CheckNamespace

namespace Highway.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension to allow for mulitple table includes
    /// </summary>
    public static class IncludeExtension
    {
        /// <summary>
        ///     Takes the specified number of records
        /// </summary>
        /// <param name="extend">Query to Extend</param>
        /// <param name="propertiesToInclude">Property of related objects to include</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Query with extension applied</returns>
        public static Query<T, TK> Include<T, TK>(this Query<T, TK> extend, params string[] propertiesToInclude)
            where T : class
        {
            foreach (string s in propertiesToInclude)
            {
                var generics = new[] {typeof (T)};
                var parameters = new Expression[] {Expression.Constant(s)};
                ((IExtendableQuery) extend).AddMethodExpression("Include", generics, parameters);
            }
            return extend;
        }


        public static IGraphQueryable<TQueried, TRelated> Include<TQueried, TRelated>(this IQueryable<TQueried> query,
            Expression<Func<TQueried, TRelated>> relatedObjectSelector) where TQueried : class
        {
            return CreateGraphQueryable<TQueried, TRelated>(query, relatedObjectSelector);
        }

        public static IGraphQueryable<TQueried, TRelated> IncludeMany<TQueried, TRelated>(
            this IQueryable<TQueried> query, Expression<Func<TQueried, IEnumerable<TRelated>>> relatedObjectSelector)
            where TQueried : class
        {
            return CreateGraphQueryable<TQueried, TRelated>(query, relatedObjectSelector);
        }

        public static IGraphQueryable<TQueried, TRelated> ThenInclude<TQueried, TFetch, TRelated>(
            this IGraphQueryable<TQueried, TFetch> query, Expression<Func<TFetch, TRelated>> relatedObjectSelector)
            where TQueried : class
        {
            return CreateGraphQueryable<TQueried, TRelated>(query, relatedObjectSelector);
        }

        public static IGraphQueryable<TQueried, TRelated> ThenIncludeMany<TQueried, TFetch, TRelated>(
            this IGraphQueryable<TQueried, TFetch> query,
            Expression<Func<TFetch, IEnumerable<TRelated>>> relatedObjectSelector) where TQueried : class
        {
            return
                CreateGraphQueryable<TQueried, TRelated>(query, relatedObjectSelector);
        }

        private static IGraphQueryable<TQueried, TRelated> CreateGraphQueryable<TQueried, TRelated>(
            IQueryable<TQueried> query, LambdaExpression relatedObjectSelector) where TQueried : class
        {
            return new GraphQueryable<TQueried, TRelated>(query, relatedObjectSelector);
        }

        public static string ParsePropertySelector<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property,
            string methodName, string paramName)
        {
            string path;
            TryParsePath(property.Body, out path);
            return path;
        }

        private static bool TryParsePath(Expression expression, out string path)
        {
            path = null;
            Expression expression1 = RemoveConvert(expression);
            var memberExpression = expression1 as MemberExpression;
            var methodCallExpression = expression1 as MethodCallExpression;
            if (memberExpression != null)
            {
                string name = memberExpression.Member.Name;
                string path1;
                if (!TryParsePath(memberExpression.Expression, out path1))
                    return false;
                path = path1 == null ? name : path1 + "." + name;
            }
            else if (methodCallExpression != null)
            {
                string path1;
                if (methodCallExpression.Method.Name == "Select" && methodCallExpression.Arguments.Count == 2 &&
                    (TryParsePath(methodCallExpression.Arguments[0], out path1) && path1 != null))
                {
                    var lambdaExpression = methodCallExpression.Arguments[1] as LambdaExpression;
                    string path2;
                    if (lambdaExpression != null && TryParsePath(lambdaExpression.Body, out path2) && path2 != null)
                    {
                        path = path1 + "." + path2;
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        private static Expression RemoveConvert(this Expression expression)
        {
            while (expression != null &&
                   (expression.NodeType == ExpressionType.Convert ||
                    expression.NodeType == ExpressionType.ConvertChecked))
                expression = RemoveConvert(((UnaryExpression) expression).Operand);
            return expression;
        }
    }

    public class GraphQueryable<T, TProperty> : GraphVistor<T>, IGraphQueryable<T, TProperty> where T : class
    {
        public GraphQueryable(IQueryable<T> queryable, Expression call) : base(queryable)
        {
        }
    }

    public class GraphVistor<T> : IQueryable<T> where T : class
    {
        private readonly IQueryable<T> _queryable;

        public GraphVistor(IQueryable<T> queryable)
        {
            _queryable = queryable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queryable.GetEnumerator();
        }

        public Expression Expression
        {
            get { return _queryable.Expression; }
        }

        public Type ElementType
        {
            get { return _queryable.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return _queryable.Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _queryable.GetEnumerator();
        }
    }

    public interface IGraphQueryable<T, TProperty> : IQueryable<T>
    {
    }
}