using Highway.Data.EventManagement;

namespace Highway.Data.Repositories
{
    public class ReadonlyDomainRepository<T> : ReadonlyRepository, IReadonlyDomainRepository<T> where T : class
    {
        private readonly ReadonlyEventManager<T> _eventManager;

        public ReadonlyDomainRepository(IReadonlyDomainContext<T> context, IDomain domain) : base(context)
        {
            _eventManager = new ReadonlyEventManager<T>(this);

            if (domain.Events == null)
            {
                return;
            }

            foreach (var @event in domain.Events)
            {
                _eventManager.Register(@event);
            }
        }

        public IReadonlyDomainContext<T> DomainContext => (IReadonlyDomainContext<T>)base.Context;
    }
}