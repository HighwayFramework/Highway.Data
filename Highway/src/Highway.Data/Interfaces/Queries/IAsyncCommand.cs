using System.Threading.Tasks;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// An Interface for Command Queries that return no value, or the return is ignored
    /// </summary>
    public interface IAsyncCommand
    {
        /// <summary>
        /// Executes the expression against the passed in context and ignores the returned value if any
        /// </summary>
        /// <param name="context">The data context that the command is executed against</param>
        Task Execute(IDataContext context);
    }
}