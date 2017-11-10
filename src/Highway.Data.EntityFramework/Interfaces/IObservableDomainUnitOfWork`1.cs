using System;
using System.Linq;

namespace Highway.Data
{
	public interface IObservableDomainUnitOfWork<out T> : IDomainUnitOfWork<T>, IObservableUnitOfWork
		where T : class, IDomain
	{
	}
}