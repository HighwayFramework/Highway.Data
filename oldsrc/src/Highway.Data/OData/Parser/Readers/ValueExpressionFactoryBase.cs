using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal abstract class ValueExpressionFactoryBase<T> : IValueExpressionFactory
    {
        public bool Handles(Type type)
        {
            return typeof (T) == type;
        }

        public abstract ConstantExpression Convert(string token);
    }
}