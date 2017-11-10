using System;

namespace Highway.Data
{
	public interface IUnitOfWork : IWriteOnlyUnitOfWork, IReadOnlyUnitOfWork, IDecorator<IUnitOfWork>
	{
	}
}