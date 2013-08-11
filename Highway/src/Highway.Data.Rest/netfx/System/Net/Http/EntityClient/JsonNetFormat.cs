using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Http
{
	/// <summary>
	/// Json format to use with <see cref="JsonNetEntityFormatter"/>.
	/// </summary>
	internal enum JsonNetFormat
	{
		/// <summary>
		/// Json text, that translates to a "text/json" content type.
		/// </summary>
		Json,

		/// <summary>
		/// Binary json, that translates to a "application/bson" content type.
		/// </summary>
		Bson,
	}
}