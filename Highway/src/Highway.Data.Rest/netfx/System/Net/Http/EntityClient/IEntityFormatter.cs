using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;

namespace System.Net.Http
{
	/// <summary>
	/// Formatter that reads and writes <see cref="HttpContent"/> 
	/// for arbitrary entity types.
	/// </summary>
	internal interface IEntityFormatter
	{
		/// <summary>
		/// Gets the content type supported by the formatter.
		/// </summary>
		string ContentType { get; }

		/// <summary>
		/// Reads an entity from <see cref="HttpContent"/>.
		/// </summary>
		T FromContent<T>(HttpContent content);

		/// <summary>
		/// Converts an entity into <see cref="HttpContent"/>.
		/// </summary>
		HttpContent ToContent<T>(T entity);
	}
}