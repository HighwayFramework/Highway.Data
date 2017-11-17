using Highway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAreaTest.Entities
{
    public class Blog : IIdentifiable<Guid>
    {
		public Blog()
		{
			Posts = new List<Post>();
		}
		public Guid Id { get; set; }
		public string Title { get; set; }
		public List<Post> Posts { get; set; }
	}
}
