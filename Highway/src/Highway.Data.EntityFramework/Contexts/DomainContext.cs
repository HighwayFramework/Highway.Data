#region

using Common.Logging.Simple;
using Highway.Data.Domain;

#endregion

namespace Highway.Data
{
    public class DomainContext<T> : DataContext, IDomainContext<T> where T : class, IDomain
    {
        public DomainContext(T domain)
            : base(domain.ConnectionString, domain.Mappings, domain.Context, new NoOpLogger())
        {
            foreach (var @event in domain.Events)
            {
                EventManager.Register(@event);
            }
        }
    }
}