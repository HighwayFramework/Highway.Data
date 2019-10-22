using System.Linq.Expressions;


// ReSharper disable CheckNamespace

namespace System.Linq.Dynamic
// ReSharper restore CheckNamespace
{
    internal class DynamicOrdering
    {
        public bool Ascending;
        public Expression Selector;
    }
}