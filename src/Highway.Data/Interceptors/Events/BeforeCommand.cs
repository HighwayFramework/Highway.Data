using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     Intercepts before a command execute
    /// </summary>
    public class BeforeCommand : EventArgs
    {
        public BeforeCommand(object command)
        {
            Command = command;
        }

        public object Command { get; set; }
    }
}
