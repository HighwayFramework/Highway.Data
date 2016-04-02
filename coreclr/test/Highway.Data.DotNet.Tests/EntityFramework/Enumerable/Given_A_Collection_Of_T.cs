using System.Collections.Generic;
using System.Linq;
using Highway.Data.Tests;
using NUnit.Framework;

namespace Highway.Data.EntityFramework.Tests.Enumerable
{
    [TestFixture]
    public class Given_A_Collection_Of_T : BaseTest<object>
    {
        [Test]
        public void When_Each_Is_Called_Action_Gets_invoked_for_every_item()
        {
            //Arrange
            var list = new List<TestActionHolder>();

            for (int i = 0; i < 20; i++)
            {
                list.Add(new TestActionHolder());
            }

            //Act
            foreach (var x in list)
            {
                x.CallMe();
            }

            //Assert
            list.All(x => x.Called);
        }


        private class TestActionHolder
        {
            public bool Called { get; set; }

            public void CallMe()
            {
                Called = true;
            }
        }
    }
}
