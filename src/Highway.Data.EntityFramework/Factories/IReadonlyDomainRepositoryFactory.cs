using System;

namespace Highway.Data.Factories
{
    public interface IReadonlyDomainRepositoryFactory
    {
        IReadonlyRepository CreateReadonly<T>()
            where T : class, IDomain;

        IReadonlyRepository CreateReadonly(Type type);
    }
}
