using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    /// Intercepts after a command execute
    /// </summary>
    public class AfterCommand : EventArgs
    {
        public object Command { get; set; }

        public AfterCommand(object command)
        {
            Command = command;
        }
    }
}
