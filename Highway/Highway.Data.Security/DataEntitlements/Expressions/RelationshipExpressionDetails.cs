using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    public class RelationshipExpressionDetails
    {
        public LambdaExpression ClosuredPredicateFactory { get; private set; }
        public LambdaExpression SelectExpression { get; set; }
    }
}