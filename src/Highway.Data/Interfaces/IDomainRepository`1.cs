using System;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public interface IDomainRepository<in T> : IReadonlyDomainRepository<T>, IRepository
        where T : class
    {
        event EventHandler<BeforeCommand> BeforeCommand;

        event EventHandler<AfterCommand> AfterCommand;

        /// <summary>
        ///     Gets the contract for a Domain Context
        /// </summary>
        new IDomainContext<T> DomainContext { get; }
    }
}
