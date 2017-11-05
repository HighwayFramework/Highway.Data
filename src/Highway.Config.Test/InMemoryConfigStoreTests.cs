using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.Config.Test
{
	[TestClass]
	public class InMemoryConfigStoreTests : IConfigStoreTests
	{
		[TestInitialize]
		public void Setup()
		{
			target = new InMemoryConfigStore(env, store);
		}
	}
}
