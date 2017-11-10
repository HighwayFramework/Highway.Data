using System;
using System.Linq;

namespace Highway.Data
{
	public class DomainRepository<T> : Repository, IDomainRepository<T> where T : class, IDomain
	{
		public DomainRepository(IDomainUnitOfWork<T> context, IDomain domain) : base(context)
		{
		}

		public IDomainUnitOfWork<T> DomainUnitOfWork => (IDomainUnitOfWork<T>)base.UnitOfWork;
	}
}