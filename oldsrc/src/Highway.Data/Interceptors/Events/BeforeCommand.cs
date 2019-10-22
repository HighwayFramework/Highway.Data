using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    /// Intercepts before a command execute
    /// </summary>
    public class BeforeCommand : EventArgs
    {
        public object Command { get; set; }

        public BeforeCommand(object command)
        {
            Command = command;
        }
    }
}