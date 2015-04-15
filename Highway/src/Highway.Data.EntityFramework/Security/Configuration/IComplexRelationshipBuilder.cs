using System;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public interface IComplexRelationshipBuilder<T>
    {
        IComplexRelationshipBuilder<T> Or(Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>> pathBuilder);
        IComplexRelationshipBuilder<T> And(Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>> pathBuilder);
    }
}