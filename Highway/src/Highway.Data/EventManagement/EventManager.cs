#region

using System.Collections.Generic;
using System.Linq;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;

#endregion

namespace Highway.Data.EventManagement
{
    /// <summary>
    ///     The base implementation of the Event manager for registration of Interceptors, and execution of them in an ordered
    ///     fashion
    /// </summary>
    public class EventManager
    {
        private readonly IObservableDataContext _context;
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        /// <summary>
        ///     Creates the event management system used internally in Highway.Data DataContexts
        /// </summary>
        /// <param name="context"></param>
        public EventManager(IObservableDataContext context)
        {
            _context = context;
            _context.BeforeSave += HandleEvent;
            _context.AfterSaved += HandleEvent;
        }

        private void HandleEvent(object sender, InterceptorEventArgs e)
        {
            foreach (var interceptor in _interceptors.Where(x => x.AppliesTo(e.EventType)).OrderBy(x => x.Priority))
            {
                InterceptorResult result = interceptor.Apply(_context, e.EventType);
                if (result.ContinueExecution == false) break;
            }
        }

        /// <summary>
        ///     Allows for the Registration of <see cref="IInterceptor" /> objects that will hook to events in priority order
        /// </summary>
        /// <param name="interceptor">The interceptor to be registered to an event</param>
        public void Register(IInterceptor interceptor)
        {
            if (_interceptors.Contains(interceptor)) return;
            _interceptors.Add(interceptor);
        }
    }
}