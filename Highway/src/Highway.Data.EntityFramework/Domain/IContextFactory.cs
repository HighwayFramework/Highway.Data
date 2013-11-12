using System;

namespace Highway.Data.Domain
{
    public interface IContextFactory
    {
        IDataContext Create<T>() where T : class, IDomain;
        IDataContext Create(Type type);
    }
}