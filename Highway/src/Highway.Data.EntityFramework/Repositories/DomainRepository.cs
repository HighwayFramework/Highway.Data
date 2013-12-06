using System;
using System.Collections.Generic;
using Highway.Data.EventManagement;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.Repositories
{
    public class DomainRepository<T> : Repository, IDomainRepository<T> where T : class, IDomain
    {
        private EventManager<T> _eventManager;

        public DomainRepository(IDomainContext<T> context, IDomain domain) : base(context)
        {
            _eventManager = new EventManager<T>(context);
            foreach (var @event in domain.Events)
            {
                _eventManager.Register(@event);
            }
        }

        public override IEnumerable<T1> Find<T1>(IQuery<T1> query)
        {
            OnBeforeQuery(new BeforeQuery());
            var result =  base.Find(query);
            OnAfterQuery(new AfterQuery());
            return result;
        }

        public event EventHandler<BeforeQuery> BeforeQuery;

        protected virtual void OnBeforeQuery(BeforeQuery e)
        {
            EventHandler<BeforeQuery> handler = BeforeQuery;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<AfterQuery> AfterQuery;

        protected virtual void OnAfterQuery(AfterQuery e)
        {
            EventHandler<AfterQuery> handler = AfterQuery;
            if (handler != null) handler(this, e);
        }
    }

    public interface IDomainRepository<T>
    {
        event EventHandler<BeforeQuery> BeforeQuery;
        event EventHandler<AfterQuery> AfterQuery;
    }
}