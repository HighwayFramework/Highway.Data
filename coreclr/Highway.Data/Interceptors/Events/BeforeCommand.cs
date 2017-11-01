using System;

namespace Highway.Data.Interceptors.Events
{
	/// <summary>
	/// Intercepts before a command execute
	/// </summary>
	public class BeforeCommand : EventArgs
	{
		public ICommand Command { get; set; }

		public BeforeCommand(ICommand command)
		{
			Command = command;
		}
	}
}