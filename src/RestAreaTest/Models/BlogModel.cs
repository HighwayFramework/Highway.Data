using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAreaTest.Models
{
    public class BlogModel
    {
		public string Title { get; set; }
		public List<PostModel> Posts { get; set; }
	}
}
