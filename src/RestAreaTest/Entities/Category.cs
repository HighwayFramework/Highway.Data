using Highway.Data;
using System;
using System.Linq;

namespace RestAreaTest.Entities
{
	public class Category : IIdentifiable<Guid>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
