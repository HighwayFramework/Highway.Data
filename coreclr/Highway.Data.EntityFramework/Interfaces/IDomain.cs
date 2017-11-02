using System;
using System.Linq;

namespace Highway.Data
{
	public interface IDomain
	{
		string ConnectionString { get; }

		IMappingConfiguration Mappings { get; }

		IContextConfiguration Context { get; }
	}
}