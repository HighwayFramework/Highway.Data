using System.Linq.Expressions;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// A collection of extension methods for extending reusable IQuery objects
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Takes the specified number of records
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuery<T> Take<T>(this IQuery<T> extend, int count)
        {
            var generics = new[] {typeof (T)};
            var parameters = new Expression[] {Expression.Constant(count)};
            ((IExtendableQuery) extend).AddMethodExpression("Take", generics, parameters);
            return extend;
        }

        /// <summary>
        /// Skip the number of items specified
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuery<T> Skip<T>(this IQuery<T> extend, int count)
        {
            var generics = new[] {typeof (T)};
            var parameters = new Expression[] {Expression.Constant(count)};
            ((IExtendableQuery) extend).AddMethodExpression("Skip", generics, parameters);
            return extend;
        }

        
    }
}