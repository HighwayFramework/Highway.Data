using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Config
{
	public interface IConfigStore
	{
		Task Clear(string key);
		Task<string> Get(string key);
		Task Set(string key, string value);
		Task<IEnumerable<string>> GetKeys();
		string GetStorageKey(string key);
		string Store { get; }
		string Environment { get; }
	}
}
