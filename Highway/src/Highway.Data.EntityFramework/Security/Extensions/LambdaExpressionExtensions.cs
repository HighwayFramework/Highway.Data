using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Highway.Data.EntityFramework.Security.Expressions;

namespace Highway.Data.EntityFramework.Security.Extensions
{
    public static class LambdaExpressionExtensions
    {
        public static LambdaExpression AndAlso(this LambdaExpression left, LambdaExpression expr2)
        {
            var parameter = left.Parameters[0];

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda(left.Type, Expression.AndAlso(left.Body, right), parameter);
        }

        public static LambdaExpression OrElse(this LambdaExpression left, LambdaExpression expr2)
        {
            var parameter = left.Parameters[0];

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda(left.Type, Expression.OrElse(left.Body, right), parameter);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof (T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof (T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left, right), parameter);
        }

        public static LambdaExpression CombineCollectionPropertySelectorWithPredicate(
            this LambdaExpression propertySelector, LambdaExpression propertyPredicate,
            CollectionBehavior collectionBehavior = CollectionBehavior.All)
        {
            // x => ids.Contains(x.Id)
            // x => Collection.Any(ids.Contains(x.Id))
            var memberExpression = propertySelector.Body as MemberExpression;
            var propertyInfo = ((PropertyInfo) memberExpression.Member);
            var containingType = propertyInfo.DeclaringType;
            if (memberExpression == null)
            {
                throw new ArgumentException("propertySelector");
            }
            var rootPredicateType = typeof (Func<,>).GetGenericTypeDefinition()
                .MakeGenericType(containingType, typeof (bool));
            MethodInfo filterMethodInfo;
            if (collectionBehavior == CollectionBehavior.All)
            {
                filterMethodInfo =
                    typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Single(mi => mi.Name == "All" && mi.GetParameters().Count() == 2)
                        .MakeGenericMethod(propertyInfo.PropertyType.ToSingleType());
            }
            else
            {
                filterMethodInfo =
                    typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Single(mi => mi.Name == "Any" && mi.GetParameters().Count() == 2)
                        .MakeGenericMethod(propertyInfo.PropertyType.ToSingleType());
            }
            var collectionCall = Expression.Call(null, filterMethodInfo, memberExpression, propertyPredicate);
            return Expression.Lambda(rootPredicateType, collectionCall, propertySelector.Parameters);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _newValue;
            private readonly Expression _oldValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }
                return base.Visit(node);
            }
        }
    }
}