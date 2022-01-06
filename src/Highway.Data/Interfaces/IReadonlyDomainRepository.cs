using System;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public interface IReadonlyDomainRepository<in T> : IReadonlyRepository
        where T : class
    {
        event EventHandler<BeforeQuery> BeforeQuery;

        event EventHandler<BeforeScalar> BeforeScalar;

        event EventHandler<AfterQuery> AfterQuery;

        event EventHandler<AfterScalar> AfterScalar;

        /// <summary>
        ///     Gets the contract for a readonly Domain Context
        /// </summary>
        IReadonlyDomainContext<T> DomainContext { get; }
    }
}
