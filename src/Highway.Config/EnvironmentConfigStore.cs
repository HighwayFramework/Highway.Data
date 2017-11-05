using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Highway.Config
{
	public class EnvironmentConfigStore : BaseConfigStore
	{
		public EnvironmentConfigStore(string env, string store) : base(env, store)
		{
		}

		public override Task Clear(string key)
		{
			System.Environment.SetEnvironmentVariable(GetStorageKey(key), null);
			return Task.FromResult(0);
		}

		public override Task<string> Get(string key)
		{
			return Task.FromResult(System.Environment.GetEnvironmentVariable(GetStorageKey(key)));

		}

		public override Task<IEnumerable<string>> GetKeys()
		{
			return Task.FromResult(
				System.Environment.GetEnvironmentVariables().Keys
				.Cast<string>()
				.Where(e => e.StartsWith(prefix))
				.Select(e => e.Replace(prefix,""))
			);
		}

		public override Task Set(string key, string value)
		{
			System.Environment.SetEnvironmentVariable(GetStorageKey(key), value);
			return Task.FromResult(0);
		}
	}
}
