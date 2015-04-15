using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class ChildSecurableRootDescriptor<T> : RootDescriptor
    {
        public ChildSecurableRootDescriptor(LambdaExpression childExpression)
        {
            ChildExpression = childExpression;
        }

        public LambdaExpression ChildExpression { get; set; }
    }
}