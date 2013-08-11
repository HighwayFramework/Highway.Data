using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Http
{
	/// <summary>
	/// Represents a remote HTTP query to an entity resource endpoint.
	/// </summary>
	/// <typeparam name="T">Type of entity being queried.</typeparam>
	internal interface IHttpEntityQuery<T> : IQueryable<T>
	{
		/// <summary>
		/// Executes the query against the remote resource endpoint.
		/// </summary>
		IHttpEntityQueryResponse<T> Execute();
	}
}
