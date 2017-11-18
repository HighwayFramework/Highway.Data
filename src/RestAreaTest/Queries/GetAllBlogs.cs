using Highway.Data;
using Microsoft.EntityFrameworkCore;
using RestAreaTest.Entities;
using System;
using System.Linq;

namespace RestAreaTest.Queries
{
	public class GetAllBlogs : Query<Blog>
	{
		public GetAllBlogs()
		{
			ContextQuery = c => c.AsQueryable<Blog>().Include(e => e.Posts);
		}
	}
}
