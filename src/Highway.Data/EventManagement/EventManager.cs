using System.Linq;

using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.EventManagement
{
    /// <summary>
    ///     The base implementation of the Event manager for registration of Interceptors, and execution of them in an ordered
    ///     fashion
    /// </summary>
    public class EventManager<T> : ReadonlyEventManager<T>
        where T : class
    {
        /// <summary>
        ///     Creates the event management system used internally in Highway.Data DataContexts
        /// </summary>
        /// <param name="repository">the repository that events will come from</param>
        public EventManager(IDomainRepository<T> repository)
            : base(repository)
        {
            Repository.AfterCommand += HandleEvent;
            Repository.DomainContext.AfterSave += HandleEvent;
            Repository.BeforeCommand += HandleEvent;
            Repository.DomainContext.BeforeSave += HandleEvent;
        }

        public new IDomainRepository<T> Repository => base.Repository as IDomainRepository<T>;

        private void HandleEvent(object sender, AfterCommand e)
        {
            var events = Interceptors.OfType<IEventInterceptor<AfterCommand>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(Repository.DomainContext, e);
                if (!result.ContinueExecution)
                {
                    break;
                }
            }
        }

        private void HandleEvent(object sender, AfterSave e)
        {
            var events = Interceptors.OfType<IEventInterceptor<AfterSave>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(Repository.DomainContext, e);
                if (!result.ContinueExecution)
                {
                    break;
                }
            }
        }

        private void HandleEvent(object sender, BeforeCommand e)
        {
            var events = Interceptors.OfType<IEventInterceptor<BeforeCommand>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(Repository.DomainContext, e);
                if (!result.ContinueExecution)
                {
                    break;
                }
            }
        }

        private void HandleEvent(object sender, BeforeSave e)
        {
            var events = Interceptors.OfType<IEventInterceptor<BeforeSave>>().OrderBy(x => x.Priority);
            foreach (var eventInterceptor in events)
            {
                var result = eventInterceptor.Apply(Repository.DomainContext, e);
                if (!result.ContinueExecution)
                {
                    break;
                }
            }
        }
    }
}
