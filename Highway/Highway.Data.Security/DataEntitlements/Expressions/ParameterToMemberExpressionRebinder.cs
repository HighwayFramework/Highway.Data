using System;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    /// <summary>
    /// Takes a direct property accessor and replaces it with a property accessor from a parameter. 
    /// <example>
    /// x => x.Foo ( x is Bar )
    /// 
    /// becomes
    /// 
    /// x => x.Bar.Foo ( x is Qux ) 
    /// </example>
    /// </summary>
    public class ParameterToMemberExpressionRebinder : ExpressionVisitor
    {
        // Modifies an expression, rebinding the usage of parameter x to the usage of a property on parameter x.
        // x => ids.Contains(x.Id)
        // x => ids.Contains(x.PropertyName.Id)

        /// <summary>
        /// Initializes an instance of <see cref="ParameterToMemberExpressionRebinder"/>.
        /// </summary>
        /// <param name="parameterExpression">The expression representing the original usage.</param>
        /// <param name="memberExpression">The epxression representing the usage to rebind to.</param>
        public ParameterToMemberExpressionRebinder(ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            _parameterExpression = parameterExpression;
            _memberExpression = memberExpression;
        }

        /// <summary>
        /// Modifies an expression, rebinding the usage of parameter x to the usage of a property on parameter x.
        /// </summary>
        /// <typeparam name="T">The Type of the new root expression.</typeparam>
        /// <typeparam name="T2">The Type of the old root expression.</typeparam>
        /// <param name="propertySelector">The expression to replace the original parameter with.</param>
        /// <param name="propertyPredicate">The predicate built on the original parameter.</param>
        /// <returns>A predicate on the rebound root expression.</returns>
        public static Expression<Func<T, bool>> CombineSinglePropertySelectorWithPredicate<T, T2>(
            Expression<Func<T, T2>> propertySelector, 
            Expression<Func<T2, bool>> propertyPredicate)
        {
            var result = CombineSinglePropertySelectorWithPredicate((LambdaExpression)propertySelector, propertyPredicate);
            return (Expression<Func<T, bool>>)result;
        }

        /// <summary>
        /// Modifies an expression, rebinding the usage of parameter x to the usage of a property on parameter x.
        /// </summary>
        /// <param name="propertySelector">The expression to replace the original parameter with.</param>
        /// <param name="propertyPredicate">The predicate built on the original parameter.</param>
        /// <returns>A predicate on the rebound root expression.</returns>
        public static LambdaExpression CombineSinglePropertySelectorWithPredicate(
            LambdaExpression propertySelector, 
            LambdaExpression propertyPredicate)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            var containingType = memberExpression.Member.DeclaringType;
            if (memberExpression == null)
            {
                throw new ArgumentException("propertySelector");
            }

            var rootPredicateType = typeof(Func<,>).GetGenericTypeDefinition().MakeGenericType(containingType, typeof(bool));
            var expr = Expression.Lambda(rootPredicateType, propertyPredicate.Body, propertySelector.Parameters);
            var rebinder = new ParameterToMemberExpressionRebinder(propertyPredicate.Parameters[0], memberExpression);
            expr = (LambdaExpression)rebinder.Visit(expr);

            return expr;
        }

        /// <summary>
        /// Visits a node and replaces it with the new member expression if it matches the expression being replaced.
        /// </summary>
        /// <param name="p">The Expression to visit.</param>
        /// <returns>The new member expression if <paramref name="p"/> is the current expression.  Otherwise returns <paramref name="p"/>.</returns>
        public override Expression Visit(Expression p)
        {
            return base.Visit(p == _parameterExpression ? _memberExpression : p);
        }

        private readonly MemberExpression _memberExpression;
        private readonly ParameterExpression _parameterExpression;
    }
}
