using System;
using System.Collections.Generic;
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

        public override IEnumerable<TProjection> Find<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
        {
            OnBeforeQuery(new BeforeQuery());
            var results = base.Find(query);
            OnAfterQuery(new AfterQuery());
            return results;
        }

        public override T1 Find<T1>(IScalar<T1> query)
        {
            OnBeforeScalar(new BeforeScalar());
            var result = base.Find(query);
            OnAfterScalar(new AfterScalar());
            return result;
        }

        public override void Execute(ICommand command)
        {
            OnBeforeCommand(new BeforeCommand());
            base.Execute(command);
            OnAfterCommand(new AfterCommand());
        }

        public event EventHandler<BeforeQuery> BeforeQuery;

        protected virtual void OnBeforeQuery(BeforeQuery e)
        {
            EventHandler<BeforeQuery> handler = BeforeQuery;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<BeforeScalar> BeforeScalar;

        protected virtual void OnBeforeScalar(BeforeScalar e)
        {
            EventHandler<BeforeScalar> handler = BeforeScalar;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<BeforeCommand> BeforeCommand;

        protected virtual void OnBeforeCommand(BeforeCommand e)
        {
            EventHandler<BeforeCommand> handler = BeforeCommand;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<AfterQuery> AfterQuery;

        protected virtual void OnAfterQuery(AfterQuery e)
        {
            EventHandler<AfterQuery> handler = AfterQuery;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<AfterScalar> AfterScalar;

        protected virtual void OnAfterScalar(AfterScalar e)
        {
            EventHandler<AfterScalar> handler = AfterScalar;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<AfterCommand> AfterCommand;

        protected virtual void OnAfterCommand(AfterCommand e)
        {
            EventHandler<AfterCommand> handler = AfterCommand;
            if (handler != null) handler(this, e);
        }
    }
}