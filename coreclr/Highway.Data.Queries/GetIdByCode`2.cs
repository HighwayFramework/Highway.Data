using System;
using System.Linq;

namespace Highway.Data.Queries
{
	public class GetIdByCode<TId, T> : Scalar<TId>
		where T : class, ICoded, IIdentifiable<TId>
		where TId : struct, IEquatable<TId>
	{
		public GetIdByCode(string code)
		{
			ContextQuery = c => c.AsQueryable<T>()
				.Where(e => e.Code == code)
				.Select(e => e.Id)
				.FirstOrDefault();
		}
	}
}
