using Highway.Data.Interceptors.Events;
using System;
using System.Linq;


namespace Highway.Data
{
    public interface IDomainRepository<in T> : IRepository where T : class
    {
        event EventHandler<BeforeQuery> BeforeQuery;

        event EventHandler<BeforeScalar> BeforeScalar;

        event EventHandler<BeforeCommand> BeforeCommand;

        event EventHandler<AfterQuery> AfterQuery;

        event EventHandler<AfterScalar> AfterScalar;

        event EventHandler<AfterCommand> AfterCommand;

        /// <summary>
        /// Gets the contract for a Domain Context
        /// </summary>
        IDomainContext<T> DomainContext { get; }
    }
}