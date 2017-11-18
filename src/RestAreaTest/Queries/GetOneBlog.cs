using Highway.Data;
using Microsoft.EntityFrameworkCore;
using RestAreaTest.Entities;
using System;
using System.Linq;

namespace RestAreaTest.Queries
{
	public class GetOneBlog : Scalar<Blog>
	{
		public GetOneBlog(Guid blogsId)
		{
			ContextQuery = c => c.AsQueryable<Blog>()
				.Where(e => e.Id == blogsId)
				.Include(e => e.Posts)
				.FirstOrDefault();
		}
	}
}
