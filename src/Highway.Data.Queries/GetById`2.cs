using System;
using System.Linq;

namespace Highway.Data.Queries
{
	public class GetById<TId, T> : Scalar<T>
		where T : class, IIdentifiable<TId>
		where TId : IEquatable<TId>
	{
		public GetById(TId id)
		{
			ContextQuery = c => c.AsQueryable<T>()
				.FirstOrDefault(x => x.Id.Equals(id));
		}
	}
}
