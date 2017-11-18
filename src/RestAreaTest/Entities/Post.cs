using Highway.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestAreaTest.Entities
{
	public class Post : IIdentifiable<Guid>
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public List<Category> Categories { get; set; }
		public Blog Blog { get; set; }
	}
}
