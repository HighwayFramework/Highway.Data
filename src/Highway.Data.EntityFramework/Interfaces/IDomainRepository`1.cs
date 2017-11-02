using System;
using System.Linq;

namespace Highway.Data
{
	public interface IDomainRepository<out T> : IRepository
		where T : class, IDomain
	{
		IDomainContext<T> DomainContext { get; }
	}
}