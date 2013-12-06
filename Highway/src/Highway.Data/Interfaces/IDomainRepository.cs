using System;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.Repositories
{
    public interface IDomainRepository<T>
    {
        event EventHandler<BeforeQuery> BeforeQuery;

        event EventHandler<BeforeScalar> BeforeScalar;

        event EventHandler<BeforeCommand> BeforeCommand;

        event EventHandler<AfterQuery> AfterQuery;

        event EventHandler<AfterScalar> AfterScalar;

        event EventHandler<AfterCommand> AfterCommand;

        IDomainContext<T> DomainContext { get; } 
    }
}