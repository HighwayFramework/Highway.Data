using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.Config
{
	public abstract class BaseConfigStore : IConfigStore
	{
		protected readonly string prefix;

		public BaseConfigStore(string env, string store)
		{
			Environment = env;
			Store = store;
			prefix = $"{Environment.ToLowerInvariant()}_{Store.ToLowerInvariant()}_";
		}

		public string Store { get; }
		public string Environment { get; }
		public string GetStorageKey(string key) => $"{prefix}{key.ToLowerInvariant()}";

		public abstract Task Clear(string key);
		public abstract Task<string> Get(string key);
		public abstract Task<IEnumerable<string>> GetKeys();
		public abstract Task Set(string key, string value);
	}
}
