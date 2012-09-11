using System;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// The interface used to interact with the Event manager implementation for registration of Interceptors
    /// </summary>
    public interface IEventManager
    {
        /// <summary>
        /// A reference to context that allows for usaged and event wiring
        /// </summary>
        IObservableDataContext Context { get; set; }

        /// <summary>
        /// Allows for the Registration of IInterceptor<typeparamref name="T"/> objects that will hook to events in priority order
        /// </summary>
        /// <param name="interceptor">The interceptor to be registered to an event</param>
        /// <typeparam name="T">The Event Args that the interceptor accepts</typeparam>
        void Register<T>(IInterceptor<T> interceptor) where T : EventArgs;
    }
}