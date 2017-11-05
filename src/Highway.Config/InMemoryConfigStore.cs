using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Config
{
	public class InMemoryConfigStore : BaseConfigStore
	{
		private readonly Dictionary<string, string> storage = new Dictionary<string, string>();
		
		public InMemoryConfigStore(string env, string store) : base(env, store)
		{
		}

		public override Task Clear(string key)
		{
			storage.Remove(GetStorageKey(key));
			return Task.FromResult(0);
		}

		public override Task<string> Get(string key)
		{
			return Task.FromResult(storage[GetStorageKey(key)]);
		}

		public override Task<IEnumerable<string>> GetKeys()
		{
			return Task.FromResult(
				storage.Keys
				.Select(e => e.Replace(prefix,""))
			);
		}

		public override Task Set(string key, string value)
		{
			storage.Add(GetStorageKey(key), value);
			return Task.FromResult(0);
		}
	}
}
