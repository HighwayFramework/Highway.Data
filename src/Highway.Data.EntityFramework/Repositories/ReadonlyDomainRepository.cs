using Highway.Data.EventManagement;

namespace Highway.Data.Repositories
{
    public class ReadonlyDomainRepository<T> : ReadonlyRepository, IReadonlyDomainRepository<T>
        where T : class, IDomain
    {
        public ReadonlyDomainRepository(IReadonlyDomainContext<T> context, IDomain domain)
            : base(context)
        {
            var eventManager = new ReadonlyEventManager<T>(this);

            if (domain.Events == null)
            {
                return;
            }

            foreach (var @event in domain.Events)
            {
                eventManager.Register(@event);
            }
        }

        public IReadonlyDomainContext<T> DomainContext => (IReadonlyDomainContext<T>)Context;
    }
}
