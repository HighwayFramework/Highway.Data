
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Highway.Data.Test.InMemory.Domain
{
	public class Blog
	{
		public Blog()
		{
			Posts = new Collection<Post>();
			Id = Guid.NewGuid();
		}

		public Blog(string title) : this()
		{
			Title = title;
		}

		public string Title { get; set; }

		public Guid Id { get; set; }

		public Author Author { get; set; }

		public ICollection<Post> Posts { get; set; }
	}
}