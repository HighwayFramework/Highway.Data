namespace Highway.Data
{
	/// <summary>
	///     An Interface for Command Queries that return no value, or the return is ignored
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		///     Executes the expression against the passed in context and ignores the returned value if any
		/// </summary>
		/// <param name="context">The data context that the command is executed against</param>
		void Execute(IDataContext context);
	}
}