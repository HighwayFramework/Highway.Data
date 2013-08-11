using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json.Bson;

namespace System.Net.Http
{
	/// <summary>
	/// An entity formatter that serializes to and from Json.
	/// </summary>
	internal class JsonNetEntityFormatter : IEntityFormatter
	{
		private JsonSerializerSettings serializerSettings;
		private JsonNetFormat format;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonNetEntityFormatter"/> class.
		/// </summary>
		/// <param name="format">The format to use when serializing or deserializing content.</param>
		/// <param name="serializerSettings">The optional serializer settings to customize Json serialization behavior.</param>
		public JsonNetEntityFormatter(JsonNetFormat format = JsonNetFormat.Json, JsonSerializerSettings serializerSettings = null)
		{
			this.format = format;
			this.serializerSettings = serializerSettings ?? new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
		}

		/// <summary>
		/// Gets the content type supported by the formatter, according to the <see cref="JsonNetFormat"/> 
		/// specified in the constructor. Is always "text/json" or "application/bson".
		/// </summary>
		public string ContentType
		{
			get { return this.format == JsonNetFormat.Json ? "text/json" : "application/bson"; }
		}

		/// <summary>
		/// Reads an entity from <see cref="HttpContent"/>.
		/// </summary>
		public T FromContent<T>(HttpContent content)
		{
			var serializer = JsonSerializer.Create(this.serializerSettings);
			var reader = this.format == JsonNetFormat.Json ?
				(JsonReader)new JsonTextReader(new StreamReader(content.ContentReadStream)) :
				(JsonReader)new BsonReader(content.ContentReadStream);

			return serializer.Deserialize<T>(reader);
		}

		/// <summary>
		/// Converts an entity into <see cref="HttpContent"/>.
		/// </summary>
		public HttpContent ToContent<T>(T entity)
		{
			var serializer = JsonSerializer.Create(this.serializerSettings);
			var stream = new MemoryStream();
			var formatting = Formatting.None;
#if DEBUG
			formatting = Formatting.Indented;
#endif
			var writer = this.format == JsonNetFormat.Json ?
				(JsonWriter)new JsonTextWriter(new StreamWriter(stream)) { Formatting = formatting } :
				(JsonWriter)new BsonWriter(stream);

			serializer.Serialize(writer, entity);
			writer.Flush();
			stream.Position = 0;

			var content = new StreamContent(stream);
			content.Headers.ContentType = new MediaTypeHeaderValue(this.ContentType);

			return content;
		}
	}
}