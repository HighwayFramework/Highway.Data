
using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;
using Highway.Data.Repositories;


namespace Highway.Data.EventManagement
{
    /// <summary>
    ///     The base implementation of the Event manager for registration of Interceptors, and execution of them in an ordered
    ///     fashion
    /// </summary>
    public class EventManager<T> where T : class
    {
        private readonly IDomainRepository<T> _repository;
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        /// <summary>
        ///     Creates the event management system used internally in Highway.Data DataContexts
        /// </summary>
        /// <param name="repository">the repository that events will come from</param>
        public EventManager(IDomainRepository<T> repository)
        {
            _repository = repository;
            _repository.DomainContext.AfterSave += HandleEvent;
            _repository.DomainContext.BeforeSave += HandleEvent;
            _repository.AfterQuery += HandleEvent;
            _repository.BeforeQuery += HandleEvent;
            _repository.BeforeCommand += HandleEvent;
            _repository.BeforeScalar += HandleEvent;
            _repository.AfterCommand += HandleEvent;
            _repository.AfterScalar += HandleEvent;

        }

        private void HandleEvent(object sender, AfterSave e)
        {
            var events = _interceptors.OfType<IEventInterceptor<AfterSave>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, BeforeSave e)
        {
            var events = _interceptors.OfType<IEventInterceptor<BeforeSave>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, BeforeQuery e)
        {
            var events = _interceptors.OfType<IEventInterceptor<BeforeQuery>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, AfterQuery e)
        {
            var events = _interceptors.OfType<IEventInterceptor<AfterQuery>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, BeforeScalar e)
        {
            var events = _interceptors.OfType<IEventInterceptor<BeforeScalar>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, AfterScalar e)
        {
            var events = _interceptors.OfType<IEventInterceptor<AfterScalar>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, BeforeCommand e)
        {
            var events = _interceptors.OfType<IEventInterceptor<BeforeCommand>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, AfterCommand e)
        {
            var events = _interceptors.OfType<IEventInterceptor<AfterCommand>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(_repository.DomainContext, e);
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