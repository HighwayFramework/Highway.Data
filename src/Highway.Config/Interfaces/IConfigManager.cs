using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Config
{
    public interface IConfigManager
    {
		Task<string> Get(string key);
		Task Set(string key, string value);
		Task Clear(string key, bool allStores);
		Task<string> GetSpecific(string env, string store, string key);
		Task SetSpecific(string env, string store, string key, string value);
		Task ClearSpecific(string env, string store, string key);
		Task<IEnumerable<string>> GetEnvironments();
		Task<IEnumerable<string>> GetStores(string env);
		Task<IEnumerable<string>> GetKeys(string env, string store);
    }
}
