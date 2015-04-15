using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class ParentSecurableRootDescriptor<T> : RootDescriptor
    {
        public ParentSecurableRootDescriptor(LambdaExpression parentExpression)
        {
            ParentExpression = parentExpression;
        }

        public LambdaExpression ParentExpression { get; set; }
    }
}