using System.Collections.Generic;
using System.Linq;

using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.EventManagement
{
    /// <summary>
    ///     The base implementation of the Event manager for registration of Interceptors, and execution of them in an ordered
    ///     fashion
    /// </summary>
    public class ReadonlyEventManager<T>
        where T : class
    {
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        private readonly IReadonlyDomainRepository<T> _repository;

        /// <summary>
        ///     Creates the event management system used internally in Highway.Data DataContexts
        /// </summary>
        /// <param name="repository">the repository that events will come from</param>
        public ReadonlyEventManager(IReadonlyDomainRepository<T> repository)
        {
            _repository = repository;
            _repository.AfterQuery += OnAfterQuery;
            _repository.BeforeQuery += OnBeforeQuery;
            _repository.BeforeScalar += OnBeforeScalar;
            _repository.AfterScalar += OnAfterScalar;
        }

        /// <summary>
        ///     Allows for the Registration of <see cref="Highway.Data.EventManagement.Interfaces.IEventInterceptor{T}" /> objects
        ///     that will hook to events in priority
        ///     order
        /// </summary>
        /// <param name="eventInterceptor">The eventInterceptor to be registered to an event</param>
        public virtual void Register(IInterceptor eventInterceptor)
        {
            if (_interceptors.Contains(eventInterceptor))
            {
                return;
            }

            _interceptors.Add(eventInterceptor);
        }

        private void ApplyInterceptors<TInterceptor>(TInterceptor e, IEnumerable<IEventInterceptor<TInterceptor>> events)
        {
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution)
                {
                    break;
                }
            }
        }

        private void OnAfterQuery(object sender, AfterQuery e)
        {
            var events = _interceptors.OfType<IEventInterceptor<AfterQuery>>().OrderBy(x => x.Priority);
            ApplyInterceptors(e, events);
        }

        private void OnAfterScalar(object sender, AfterScalar e)
        {
            var events = _interceptors.OfType<IEventInterceptor<AfterScalar>>().OrderBy(x => x.Priority);
            ApplyInterceptors(e, events);
        }

        private void OnBeforeQuery(object sender, BeforeQuery e)
        {
            var events = _interceptors.OfType<IEventInterceptor<BeforeQuery>>().OrderBy(x => x.Priority);
            ApplyInterceptors(e, events);
        }

        private void OnBeforeScalar(object sender, BeforeScalar e)
        {
            var events = _interceptors.OfType<IEventInterceptor<BeforeScalar>>().OrderBy(x => x.Priority);
            ApplyInterceptors(e, events);
        }
    }
}
