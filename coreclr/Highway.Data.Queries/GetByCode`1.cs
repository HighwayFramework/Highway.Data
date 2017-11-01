using System;
using System.Linq;

namespace Highway.Data.Queries
{
	public class GetByCode<T> : Scalar<T>
		where T : class, ICoded
	{
		public GetByCode(string code)
		{
			ContextQuery = c => c.AsQueryable<T>()
				.FirstOrDefault(e => e.Code == code);
		}
	}
}
