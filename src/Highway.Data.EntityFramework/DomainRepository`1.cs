using System;
using System.Linq;

namespace Highway.Data
{
	public class DomainRepository<T> : Repository, IDomainRepository<T> where T : class, IDomain
	{
		public DomainRepository(IDomainContext<T> context, IDomain domain) : base(context)
		{
		}

		public IDomainContext<T> DomainContext => (IDomainContext<T>)base.Context;
	}
}