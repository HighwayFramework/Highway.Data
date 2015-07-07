using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Map a predicate expression to a different source parameter.
    /// </summary>
    public class PredicateMapper
    {
        private readonly IList<IPredicateConverter> _converters;

        private PredicateMapper(IPredicateConverter converter)
        {
            _converters = new List<IPredicateConverter> {converter};
        }

        /// <summary>
        ///     Create a map based on the passed type arguments.
        /// </summary>
        /// <typeparam name="TSource">The source parameter type.</typeparam>
        /// <typeparam name="TTarget">The target parameter type.</typeparam>
        /// <returns>An instance of a <see cref="PredicateMapper" /></returns>
        public static PredicateMapper Map<TSource, TTarget>()
        {
            return new PredicateMapper(new PredicateConverter<TSource, TTarget>());
        }

        /// <summary>
        ///     Adds the defined mapping to the mapper.
        /// </summary>
        /// <typeparam name="TSource">The source parameter type.</typeparam>
        /// <typeparam name="TTarget">The target parameter type.</typeparam>
        /// <returns>An instance of a <see cref="PredicateMapper" />.</returns>
        public PredicateMapper And<TSource, TTarget>()
        {
            if (_converters.All(x => x.SourceType != typeof (TSource)))
            {
                _converters.Add(new PredicateConverter<TSource, TTarget>());
            }

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <typeparam name="TSource">The source parameter type.</typeparam>
        /// <typeparam name="TTarget">The target parameter type.</typeparam>
        /// <typeparam name="TValue">The return type for the member.</typeparam>
        /// <returns>An instance of a <see cref="PredicateMapper" /></returns>
        /// <exception cref="ArgumentException">Thrown if the passed expression does not map to a member.</exception>
        public PredicateMapper MapMember<TSource, TTarget, TValue>(
            Expression<Func<TSource, TValue>> source,
            Expression<Func<TTarget, TValue>> result)
        {
            var memberBody = result.Body as MemberExpression;
            if (memberBody == null)
            {
                throw new ArgumentException("Expression should access member.", "result");
            }

            var sourceBody = source.Body as MemberExpression;
            if (sourceBody == null)
            {
                throw new ArgumentException("Expression should access member.", "source");
            }

            var resultMember = memberBody.Member;
            var sourceMember = sourceBody.Member;

            var converter =
                _converters.FirstOrDefault(x => x.SourceType == typeof (TSource) && x.TargetType == typeof (TTarget));
            if (converter == null)
            {
                converter = new PredicateConverter<TSource, TTarget>();
                _converters.Add(converter);
            }

            converter.Substitutions[sourceMember] = resultMember;

            return this;
        }

        /// <summary>
        ///     Converts the passed expression tree based on the known mappings.
        /// </summary>
        /// <param name="predicate">The expression tree to convert.</param>
        /// <typeparam name="TSource">The source parameter type.</typeparam>
        /// <typeparam name="TTarget">The target parameter type.</typeparam>
        /// <returns>An expression tree for the target parameter.</returns>
        public Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(Expression<Func<TSource, bool>> predicate)
        {
            var visitor = new InnerVisitor(_converters);

            var expression = visitor.Visit(predicate);
            return (Expression<Func<TTarget, bool>>) expression;
        }

        private class InnerVisitor : ExpressionVisitor
        {
            private readonly IPredicateConverter[] _converters;

            public InnerVisitor(IEnumerable<IPredicateConverter> converters)
            {
                _converters = converters.ToArray();
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                if (node.Parameters.Count != 1)
                {
                    return base.VisitLambda(node);
                }

                var converter = GetConverter(node.Parameters[0].Type);
                if (converter == null)
                {
                    return base.VisitLambda(node);
                }

                var body = Visit(node.Body);
                var parameters = node.Parameters.Select(Visit).Cast<ParameterExpression>();

                return Expression.Lambda(body, parameters);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                var converter = GetConverter(node.Type);
                if (converter == null)
                {
                    return base.VisitParameter(node);
                }

                return Expression.Parameter(converter.TargetType, node.Name);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var method = node.Method;
                var declaringType = method.DeclaringType;
                if (method.IsStatic)
                {
                    declaringType = method.GetGenericArguments()[0];
                }

                var converter = GetConverter(declaringType);
                if (converter == null)
                {
                    return base.VisitMethodCall(node);
                }

                var member = (MethodInfo) GetSubstitution(converter, method);
                var convertedExpression = Visit(node.Object);
                var convertedArguments = node.Arguments.Select(Visit).ToArray();
                return Expression.Call(convertedExpression, member, convertedArguments);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var memberInfo = node.Member;
                var converter = GetConverter(node.Expression.Type);
                if (converter == null)
                {
                    return base.VisitMember(node);
                }

                var member = GetSubstitution(converter, memberInfo);
                var convertedExpression = Visit(node.Expression);
                return Expression.MakeMemberAccess(convertedExpression, member);
            }

            private static MemberInfo GetDefaultMemberSubstitution(Type targetType, MemberInfo memberInfo)
            {
                return
                    targetType.GetMember(memberInfo.Name, memberInfo.MemberType,
                        BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)[0];
            }

            private static MethodInfo GetDefaultMethodSubstitution(Type sourceType, Type targetType,
                MethodInfo methodInfo)
            {
                if (methodInfo.IsStatic && methodInfo.IsGenericMethod)
                {
                    var genericDefinition = methodInfo.GetGenericMethodDefinition();
                    return genericDefinition.MakeGenericMethod(
                        methodInfo.GetGenericArguments()
                            .Replace(t => t == sourceType, targetType)
                            .ToArray());
                }

                return targetType.GetMethod(
                    methodInfo.Name,
                    methodInfo.GetParameters().Select(x => x.ParameterType).ToArray());
            }

            private static MemberInfo GetSubstitution(IPredicateConverter converter, MethodInfo method)
            {
                return converter.Substitutions.ContainsKey(method)
                    ? converter.Substitutions[method]
                    : GetDefaultMethodSubstitution(converter.SourceType, converter.TargetType, method);
            }

            private static MemberInfo GetSubstitution(IPredicateConverter converter, MemberInfo member)
            {
                return converter.Substitutions.ContainsKey(member)
                    ? converter.Substitutions[member]
                    : GetDefaultMemberSubstitution(converter.TargetType, member);
            }

            private IPredicateConverter GetConverter(Type type)
            {
                return _converters.FirstOrDefault(x => x.SourceType == type);
            }
        }
    }
}