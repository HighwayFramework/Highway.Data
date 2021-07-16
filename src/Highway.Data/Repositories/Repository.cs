using Highway.Data.Interceptors.Events;
using System;
using System.Threading.Tasks;

namespace Highway.Data
{
    /// <summary>
    ///     A Entity Framework Specific repository implementation that uses Specification pattern to execute Queries in a
    ///     controlled fashion.
    /// </summary>
    public class Repository : ReadonlyRepository, IRepository
    {
        /// <summary>
        ///     Creates a Repository that uses the context provided
        /// </summary>
        /// <param name="context">The data context that this repository uses</param>
        public Repository(IDataContext context)
            : base(context)
        {
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" />
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        public virtual void Execute(ICommand command)
        {
            OnBeforeCommand(new BeforeCommand(command));
            command.Execute(_context);
            OnAfterCommand(new AfterCommand(command));
        }

        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" /> asynchronously
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        public virtual Task ExecuteAsync(ICommand command)
        {
            var task = new Task(() => command.Execute(_context));
            task.Start();
            return task;
        }

        public event EventHandler<BeforeCommand> BeforeCommand;

        protected virtual void OnBeforeCommand(BeforeCommand e)
        {
            BeforeCommand?.Invoke(this, e);
        }

        public event EventHandler<AfterCommand> AfterCommand;

        protected virtual void OnAfterCommand(AfterCommand e)
        {
            AfterCommand?.Invoke(this, e);
        }
    }
}