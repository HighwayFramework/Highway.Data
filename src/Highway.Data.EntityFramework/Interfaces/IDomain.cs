using System;
using System.Linq;

namespace Highway.Data
{
	public interface IDomain
	{
		string ConnectionString { get; }

		IMappingConfiguration Mappings { get; }

		IUnitOfWorkConfiguration Context { get; }
	}
}