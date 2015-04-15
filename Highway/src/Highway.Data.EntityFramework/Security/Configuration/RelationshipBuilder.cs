using System;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class RelationshipBuilder<T> : IComplexRelationshipBuilder<T>, IRelationshipBuilder
    {
        private Relationship _relationship;

        public IComplexRelationshipBuilder<T> Or(Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>> pathBuilder)
        {
            var builder = new PathBuilder<T>();
            pathBuilder(builder);
            _relationship = new ComplexAndRelationship(_relationship,
                new SimpleRelationship(builder.Build(), typeof (T)));
            return this;
        }

        public IComplexRelationshipBuilder<T> And(Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>> pathBuilder)
        {
            var builder = new PathBuilder<T>();
            pathBuilder(builder);
            _relationship = new ComplexAndRelationship(_relationship,
                new SimpleRelationship(builder.Build(), typeof (T)));
            return this;
        }

        public Relationship Build()
        {
            return _relationship;
        }

        public IComplexRelationshipBuilder<T> By(Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>> pathBuilder)
        {
            var builder = new PathBuilder<T>();
            pathBuilder(builder);
            _relationship = new SimpleRelationship(builder.Build(), typeof (T));
            return this;
        }
    }
}