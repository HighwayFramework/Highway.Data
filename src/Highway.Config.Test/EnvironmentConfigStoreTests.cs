using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.Config.Test
{
	[TestClass]
	public class EnvironmentConfigStoreTests : IConfigStoreTests
	{
		[TestInitialize]
		public async Task Setup()
		{
			target = new EnvironmentConfigStore(env, store);
			var keys = await target.GetKeys();
			foreach (var key in keys)
			{
				await target.Clear(key);
			}
		}
	}
}
