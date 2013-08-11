using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Http
{
	/// <summary>
	/// Represents an HTTP query response.
	/// </summary>
	/// <typeparam name="T">Type of entity that was queried.</typeparam>
	internal interface IHttpEntityQueryResponse<T> : IEnumerable<T>
	{
		/// <summary>
		/// Gets the original response from the service.
		/// </summary>
		HttpResponseMessage Response { get; }

		/// <summary>
		/// Gets the optional total count of items on the service side, 
		/// based on the value of the <c>X-TotalCount</c> header.
		/// </summary>
		long? TotalCount { get; }
	}
}
