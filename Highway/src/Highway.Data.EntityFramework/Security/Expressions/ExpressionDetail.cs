using System;

namespace Highway.Data.EntityFramework.Security.Expressions
{
    public class ExpressionDetail
    {
        public ExpressionDetail(Delegate predicateFactory, Delegate rootIdsExpression)
        {
            PredicateFactory = predicateFactory;
            RootIdsExpression = rootIdsExpression;
        }

        public Delegate PredicateFactory { get; private set; }
        public Delegate RootIdsExpression { get; private set; }
    }
}