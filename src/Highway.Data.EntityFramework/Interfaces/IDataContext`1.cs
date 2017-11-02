using System;
using System.Linq;

namespace Highway.Data
{
	public interface IDomainContext<out T> : IDataContext
		where T : class, IDomain
	{
		T Domain { get; }
	}
}