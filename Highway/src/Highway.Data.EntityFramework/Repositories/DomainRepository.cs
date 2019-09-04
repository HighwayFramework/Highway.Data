using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EventManagement;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.Repositories
{
    public class DomainRepository<T> : Repository, IDomainRepository<T> where T : class, IDomain
    {
        private EventManager<T> _eventManager;

        public IDomainContext<T> DomainContext
        {
            get { return (IDomainContext<T>) base.Context;}
        } 

        public DomainRepository(IDomainContext<T> context, IDomain domain) : base(context)
        {
            _eventManager = new EventManager<T>(this);

            if (domain.Events == null)
            {
                return;
            }

            foreach (var @event in domain.Events)
            {
                _eventManager.Register(@event);
            }
        }
    }
}