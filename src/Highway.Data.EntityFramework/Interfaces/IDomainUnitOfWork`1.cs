using System;
using System.Linq;

namespace Highway.Data
{
	public interface IDomainUnitOfWork<out T> : IUnitOfWork
		where T : class, IDomain
	{
		T Domain { get; }
	}
}