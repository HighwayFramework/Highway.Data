using System;
using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.EventManagement
{
    /// <summary>
    /// Event Args fired to inform the interceptors what event type fired
    /// </summary>
    public class InterceptorEventArgs : EventArgs
    {
        public EventType EventType { get; set; }

        public InterceptorEventArgs(EventType eventType)
        {
            EventType = eventType;
        }

        /// <summary>
        /// Simple Factory Method for fluent creation
        /// </summary>
        /// <param name="eventType">the event being created for</param>
        /// <returns>an instance of the InterceptorEventArgs with the event set</returns>
        public static InterceptorEventArgs ForEvent(EventType eventType)
        {
            return new InterceptorEventArgs(eventType);
        }
    }
}