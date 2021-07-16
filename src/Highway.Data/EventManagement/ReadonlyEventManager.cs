﻿using System.Collections.Generic;
using System.Linq;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.EventManagement
{
    public class ReadonlyEventManager<T> where T : class
    {
        protected List<IInterceptor> Interceptors { get; } = new List<IInterceptor>();

        public ReadonlyEventManager(IReadonlyDomainRepository<T> repository)
        {
            Repository = repository;
            Repository.AfterQuery += HandleEvent;
            Repository.BeforeQuery += HandleEvent;
        }

        protected IReadonlyDomainRepository<T> Repository { get; }

        private void HandleEvent(object sender, BeforeQuery e)
        {
            var events = Interceptors.OfType<IEventInterceptor<BeforeQuery>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(Repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        private void HandleEvent(object sender, AfterQuery e)
        {
            var events = Interceptors.OfType<IEventInterceptor<AfterQuery>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(Repository.DomainContext, e);
                if (!result.ContinueExecution) break;
            }
        }

        /// <summary>
        ///     Allows for the Registration of <see cref="IEventInterceptor{T}" /> objects that will hook to events in priority order
        /// </summary>
        /// <param name="eventInterceptor">The eventInterceptor to be registered to an event</param>
        public void Register(IInterceptor eventInterceptor)
        {
            if (Interceptors.Contains(eventInterceptor)) return;
            Interceptors.Add(eventInterceptor);
        }
    }
}