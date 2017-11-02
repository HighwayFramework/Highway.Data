using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Config
{
	public class InMemoryConfigStore : IConfigStore
	{
		private readonly Dictionary<string, string> storage = new Dictionary<string, string>();

		public Task Clear(string key)
		{
			storage.Remove(key);
			return Task.FromResult(0);
		}

		public Task<string> Get(string key)
		{
			return Task.FromResult(storage[key]);
		}

		public Task<IEnumerable<string>> GetKeys()
		{
			return Task.FromResult(storage.Keys as IEnumerable<string>);
		}

		public Task Set(string key, string value)
		{
			storage.Add(key, value);
			return Task.FromResult(0);
		}
	}
}
