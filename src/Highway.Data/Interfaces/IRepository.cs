using System.Collections.Generic;
using System.Threading.Tasks;


namespace Highway.Data
{
    /// <summary>
    ///     The interface used to interact with the ORM Specific Implementations
    /// </summary>
    public interface IRepository : IReadonlyRepository
    {
        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" />
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        void Execute(ICommand command);

        /// <summary>
        ///     Executes a prebuilt <see cref="ICommand" /> asynchronously
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        Task ExecuteAsync(ICommand command);
    }
}