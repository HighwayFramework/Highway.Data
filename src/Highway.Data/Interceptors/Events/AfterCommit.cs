
using System;


namespace Highway.Data.Interceptors.Events
{
	/// <summary>
	///     The Event Arguments for a Post-Save Interceptor to use
	/// </summary>
	public class AfterCommit : EventArgs
	{
		public AfterCommit(int? changes, Exception exception)
		{
			Changes = changes;
			Exception = exception;
		}
		public int? Changes { get; set; }
		public Exception Exception { get; set; }
	}
}