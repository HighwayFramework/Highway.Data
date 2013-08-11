using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Http
{
	/// <summary>
	/// Provides the <see cref="Execute"/> extension method to execute 
	/// the query and retrieve the additional response information like 
	/// total count and the original <see cref="HttpResponseMessage"/>.
	/// </summary>
	internal static class QueryableExtension
	{
		/// <summary>
		/// Executes the HTTP query and retrieves the full response information 
		/// together with the actual result.
		/// </summary>
		public static IHttpEntityQueryResponse<T> Execute<T>(this IQueryable<T> query)
		{
			// This condition makes this easier to unit test code that 
			// uses this extension method.
			if (!(query is IHttpEntityQuery<T>))
				return new InMemoryQueryResponse<T>(query.ToList());

			var httpQuery = (IHttpEntityQuery<T>)query;

			return httpQuery.Execute();
		}

		private class InMemoryQueryResponse<T> : IHttpEntityQueryResponse<T>
		{
			private IList<T> result;

			public InMemoryQueryResponse(IList<T> result)
			{
				this.result = result;
			}

			public HttpResponseMessage Response
			{
				get { return new HttpResponseMessage(HttpStatusCode.OK, "OK"); }
			}

			public long? TotalCount
			{
				get { return this.result.Count; }
			}

			public IEnumerator<T> GetEnumerator()
			{
				return this.result.GetEnumerator();
			}

			Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}
