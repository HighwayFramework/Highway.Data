using System;
using System.Linq.Expressions;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IExtendableQuery
    {
        void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters);
    }
}