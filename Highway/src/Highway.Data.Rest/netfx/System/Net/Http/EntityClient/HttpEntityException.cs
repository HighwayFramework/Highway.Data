using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net;

namespace System.Net.Http
{
	/// <summary>
	/// Exception that occurs when the response static is not 
	/// successfull, therefore preventing deserialization of 
	/// the expected entities.
	/// </summary>
	internal class HttpEntityException : HttpException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpEntityException"/> class.
		/// </summary>
		public HttpEntityException(HttpResponseMessage response)
			: base(response.ReasonPhrase)
		{
			this.Response = response;
		}

		/// <summary>
		/// Gets the status code of the <see cref="Response"/>.
		/// </summary>
		public HttpStatusCode StatusCode { get { return this.Response.StatusCode; } }

		/// <summary>
		/// Gets the full response object that caused this exception.
		/// </summary>
		public HttpResponseMessage Response { get; private set; }
	}
}