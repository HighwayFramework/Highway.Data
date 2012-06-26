using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace FrameworkExtension.Core.QueryProviders
{
    public class QueryTranslator<T> : IOrderedQueryable<T>
    {
        private Expression expression = null;
        private QueryTranslatorProvider<T> provider = null;

        public QueryTranslator(IQueryable source)
        {
            expression = Expression.Constant(this);
            provider = new QueryTranslatorProvider<T>(source);
        }

        public QueryTranslator(IQueryable source, Expression e)
        {
            if (e == null) throw new ArgumentNullException("e");
            expression = e;
            provider = new QueryTranslatorProvider<T>(source);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)provider.ExecuteEnumerable(this.expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return provider.ExecuteEnumerable(this.expression).GetEnumerator();
        }

        public QueryTranslator<T> Include(String path)
        {
            ObjectQuery<T> possibleObjectQuery = provider.source as ObjectQuery<T>;
            if (possibleObjectQuery != null)
            {
                return new QueryTranslator<T>(possibleObjectQuery.Include(path));
            }
            else
            {
                throw new InvalidOperationException("The Include should only happen at the beginning of a LINQ expression");
            }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return expression; }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }
    }

    public class QueryTranslatorProvider<T> : ExpressionVisitor, IQueryProvider
    {
        internal IQueryable source;

        public QueryTranslatorProvider(IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return new QueryTranslator<TElement>(source, expression) as IQueryable<TElement>;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            Type elementType = expression.Type.GetGenericArguments().First();
            IQueryable result = (IQueryable)Activator.CreateInstance(typeof(QueryTranslator<>).MakeGenericType(elementType),
                    new object[] { source, expression });
            return result;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            object result = (this as IQueryProvider).Execute(expression);
            return (TResult)result;
        }

        public object Execute(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression translated = this.Visit(expression);
            return source.Provider.Execute(translated);
        }

        internal IEnumerable ExecuteEnumerable(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression translated = this.Visit(expression);
            return source.Provider.CreateQuery(translated);
        }

        #region Visitors
        protected override Expression VisitConstant(ConstantExpression c)
        {
            // fix up the Expression tree to work with EF again
            if (c.Type == typeof(QueryTranslator<T>))
            {
                return source.Expression;
            }
            else
            {
                return base.VisitConstant(c);
            }
        }
        #endregion
    }
}