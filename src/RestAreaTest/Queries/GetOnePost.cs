using Highway.Data;
using RestAreaTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAreaTest.Queries
{
	public class GetOnePost : Scalar<Post>
    {
		public GetOnePost(Guid postsId)
		{
			ContextQuery = c => c.AsQueryable<Post>()
				.Where(e => e.Id == postsId)
				.FirstOrDefault();
		}
		public GetOnePost(Guid blogsId, Guid postsId)
		{
			ContextQuery = c => c.AsQueryable<Post>()
				.Where(e => e.Id == postsId)
				.Where(e => e.Blog.Id == blogsId)
				.FirstOrDefault();
		}
    }
}
