#region

using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;

#endregion

namespace Highway.Data.EventManagement
{
    /// <summary>
    ///     The base implementation of the Event manager for registration of Interceptors, and execution of them in an ordered
    ///     fashion
    /// </summary>
    public class EventManager<T> where T : class
    {
        private readonly IDomainContext<T> _context;
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        /// <summary>
        ///     Creates the event management system used internally in Highway.Data DataContexts
        /// </summary>
        /// <param name="context"></param>
        public EventManager(IDomainContext<T> context)
        {
            _context = context;
            _context.BeforeSave += HandleEvent;
            _context.AfterSaved += HandleEvent;
        }

        private void HandleEvent<TEvent>(object sender, TEvent e) where TEvent : EventArgs
        {
            var events = _interceptors.OfType<IEventInterceptor<TEvent>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_context, e);
                if (!result.ContinueExecution) break;
            }
        }

        /// <summary>
        ///     Allows for the Registration of <see cref="IEventInterceptor{T}" /> objects that will hook to events in priority order
        /// </summary>
        /// <param name="eventInterceptor">The eventInterceptor to be registered to an event</param>
        public void Register(IInterceptor eventInterceptor)
        {
            if (_interceptors.Contains(eventInterceptor)) return;
            _interceptors.Add(eventInterceptor);
        }
    }
}