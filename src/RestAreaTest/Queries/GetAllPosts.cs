using Highway.Data;
using RestAreaTest.Entities;
using System;
using System.Linq;

namespace RestAreaTest.Queries
{
	public class GetAllPosts : Query<Post>
	{
		public GetAllPosts()
		{
			ContextQuery = c => c.AsQueryable<Post>();
		}

		public GetAllPosts(Guid blogsId)
		{
			ContextQuery = c => c.AsQueryable<Post>()
				.Where(e => e.Blog.Id == blogsId);
		}
	}
}
