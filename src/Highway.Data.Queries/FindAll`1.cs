using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Highway.Data.Queries
{
    public class FindAll<T> : Query<T> 
		where T : class
    {
		public FindAll()
		{
			ContextQuery = c => c.AsQueryable<T>();
		}
    }
}
