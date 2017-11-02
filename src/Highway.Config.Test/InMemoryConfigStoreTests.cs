using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.Config.Test
{
    [TestClass]
    public class InMemoryConfigStoreTests
    {
		private InMemoryConfigStore target;
		[TestInitialize]
		public void Setup()
		{
			target = new InMemoryConfigStore();
		}

		[TestMethod]
        public async Task GetKeys_Should_Return_Empty_Enumerable()
        {
			// Arrange

			// Act
			var result = await target.GetKeys();

			// Assert
			Assert.AreEqual(0, result.Count());

		}

		[TestMethod]
		public async Task Set_Should_Add_To_GetKeys_Result()
		{
			// Arrange

			// Act
			await target.Set("test", "nothing");

			// Assert
			var result = await target.GetKeys();
			Assert.AreEqual(1, result.Count());

		}

		[TestMethod]
		public async Task Get_Should_Return_Value()
		{
			// Arrange
			await target.Set("test", "nothing");

			// Act
			var result = await target.Get("test");

			// Assert
			Assert.AreEqual("nothing", result);

		}

		[TestMethod]
		public async Task Clear_Should_Result_In_Lower_Count()
		{
			// Arrange
			await target.Set("test", "nothing");

			// Act
			await target.Clear("test");

			// Assert
			var count = (await target.GetKeys()).Count();
			Assert.AreEqual(0, count);

		}
	}
}
