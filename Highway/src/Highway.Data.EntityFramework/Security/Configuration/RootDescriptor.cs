using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class RootDescriptor : IRelationshipBuilder
    {
        public Type SecuredBy { get; set; }
        public LambdaExpression SelfExpression { get; set; }

        public Relationship Build()
        {
            return new SimpleRelationship(
                new SimplePathDescriptor(SecuredBy, SecuredBy, SelfExpression, WhenNull.Deny), SecuredBy);
        }
    }
}