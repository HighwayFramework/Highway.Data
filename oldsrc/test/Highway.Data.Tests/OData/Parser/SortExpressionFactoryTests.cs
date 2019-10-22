// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the SortExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Highway.Data.OData;
using Highway.Data.OData.Parser;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser
{
    public class SortExpressionFactoryTests
	{
		private SortExpressionFactoryTests()
		{
		}

		[TestFixture]
		public class FakeItemSortExpressionFactoryTests
		{
			private FakeItem[] _items;
			private SortExpressionFactory _factory;

			[SetUp]
			public void TestSetup()
			{
				_items = new[]
						 {
							 new FakeItem { IntValue = 2, DoubleValue = 5, StringValue = "aa" },
							 new FakeItem { IntValue = 1, DoubleValue = 4, StringValue = "a" },
							 new FakeItem { IntValue = 3, DoubleValue = 4, StringValue = "aaa" }
						 };
			}

			[TestFixtureSetUp]
			public void FixtureSetup()
			{
				_factory = new SortExpressionFactory(new MemberNameResolver());
			}

			[Test]
			public void WhenFilterContainSortDescriptionWithDirectionThenCreatesMatchingSortDescription()
			{
				const string Orderstring = "IntValue desc";

				var descriptions = _factory.Create<FakeItem>(Orderstring);
				var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(
					3,
					sortedItems.OfType<FakeItem>()
						.ElementAt(0)
						.IntValue);
				Assert.AreEqual(
					2,
					sortedItems.OfType<FakeItem>()
						.ElementAt(1)
						.IntValue);
				Assert.AreEqual(
					1,
					sortedItems.OfType<FakeItem>()
						.ElementAt(2)
						.IntValue);
			}

			[Test]
			public void WhenFilterContainSortDescriptionWithoutDirectionThenCreatesMatchingAscendingSortDescription()
			{
				const string Orderstring = "IntValue";

				var descriptions = _factory.Create<FakeItem>(Orderstring);
				var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(
					1,
					sortedItems.OfType<FakeItem>()
						.ElementAt(0)
						.IntValue);
				Assert.AreEqual(
					2,
					sortedItems.OfType<FakeItem>()
						.ElementAt(1)
						.IntValue);
				Assert.AreEqual(
					3,
					sortedItems.OfType<FakeItem>()
						.ElementAt(2)
						.IntValue);
			}

			[Test]
			public void WhenFilterContainsSortMultipleDescriptionsThenSortsByAll()
			{
				const string Orderstring = "DoubleValue,IntValue desc";

				var descriptions = _factory.Create<FakeItem>(Orderstring);
				var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(
					3,
					sortedItems.OfType<FakeItem>()
						.ElementAt(0)
						.IntValue);
				Assert.AreEqual(
					1,
					sortedItems.OfType<FakeItem>()
						.ElementAt(1)
						.IntValue);
				Assert.AreEqual(
					2,
					sortedItems.OfType<FakeItem>()
						.ElementAt(2)
						.IntValue);
			}

			[Test]
			public void WhenFilterContainsSortMultipleDescriptionsWithSpaceBetweenThenSortsByAll()
			{
				const string Orderstring = "DoubleValue, IntValue desc";

				var descriptions = _factory.Create<FakeItem>(Orderstring);
				var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(
					3,
					sortedItems.OfType<FakeItem>()
						.ElementAt(0)
						.IntValue);
				Assert.AreEqual(
					1,
					sortedItems.OfType<FakeItem>()
						.ElementAt(1)
						.IntValue);
				Assert.AreEqual(
					2,
					sortedItems.OfType<FakeItem>()
						.ElementAt(2)
						.IntValue);
			}

			[Test]
			public void WhenFilterIsEmptyThenDoesNotSort()
			{
				var descriptions = _factory.Create<FakeItem>(string.Empty);
				var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(
					2,
					sortedItems.OfType<FakeItem>()
						.ElementAt(0)
						.IntValue);
				Assert.AreEqual(
					1,
					sortedItems.OfType<FakeItem>()
						.ElementAt(1)
						.IntValue);
				Assert.AreEqual(
					3,
					sortedItems.OfType<FakeItem>()
						.ElementAt(2)
						.IntValue);
			}

			[Test]
			public void WhenOrderingByChildPropertyThenUsesChildProperty()
			{
				const string Orderstring = "StringValue/Length desc";

				var descriptions = _factory.Create<FakeItem>(Orderstring);
				var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1, false);
				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(
					"aaa",
					sortedItems.OfType<FakeItem>()
						.ElementAt(0)
						.StringValue);
				Assert.AreEqual(
					"aa",
					sortedItems.OfType<FakeItem>()
						.ElementAt(1)
						.StringValue);
				Assert.AreEqual(
					"a",
					sortedItems.OfType<FakeItem>()
						.ElementAt(2)
						.StringValue);
			}
		}

		[TestFixture]
		public class AliasItemSortExpressionFactoryTests
		{
			private AliasItem[] _items;
			private SortExpressionFactory _factory;

			[SetUp]
			public void TestSetup()
			{
				_items = new[]
						 {
							 new AliasItem { AliasIntValue = 2, AliasDoubleValue = 5, StringValue = "aa" },
							 new AliasItem { AliasIntValue = 1, AliasDoubleValue = 4, StringValue = "a" },
							 new AliasItem { AliasIntValue = 3, AliasDoubleValue = 4, StringValue = "aaa" }
						 };
			}

			[TestFixtureSetUp]
			public void FixtureSetup()
			{
				_factory = new SortExpressionFactory(new MemberNameResolver());
			}

			[Test]
			public void WhenFilterContainSortDescriptionWithDirectionThenCreatesMatchingSortDescription()
			{
				const string Orderstring = "IntValue desc";

				var descriptions = _factory.Create<AliasItem>(Orderstring);
                var filter = new ModelFilter<AliasItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(3, sortedItems.OfType<AliasItem>().ElementAt(0).AliasIntValue);
				Assert.AreEqual(2, sortedItems.OfType<AliasItem>().ElementAt(1).AliasIntValue);
				Assert.AreEqual(1, sortedItems.OfType<AliasItem>().ElementAt(2).AliasIntValue);
			}

			[Test]
			public void WhenFilterContainSortDescriptionWithoutDirectionThenCreatesMatchingAscendingSortDescription()
			{
				const string Orderstring = "IntValue";

				var descriptions = _factory.Create<AliasItem>(Orderstring);
                var filter = new ModelFilter<AliasItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(1, sortedItems.OfType<AliasItem>().ElementAt(0).AliasIntValue);
				Assert.AreEqual(2, sortedItems.OfType<AliasItem>().ElementAt(1).AliasIntValue);
				Assert.AreEqual(3, sortedItems.OfType<AliasItem>().ElementAt(2).AliasIntValue);
			}

			[Test]
			public void WhenFilterContainsSortMultipleDescriptionsThenSortsByAll()
			{
				const string Orderstring = "DoubleValue,IntValue desc";

				var descriptions = _factory.Create<AliasItem>(Orderstring);
                var filter = new ModelFilter<AliasItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(3, sortedItems.OfType<AliasItem>().ElementAt(0).AliasIntValue);
				Assert.AreEqual(1, sortedItems.OfType<AliasItem>().ElementAt(1).AliasIntValue);
				Assert.AreEqual(2, sortedItems.OfType<AliasItem>().ElementAt(2).AliasIntValue);
			}

			[Test]
			public void WhenFilterContainsSortMultipleDescriptionsWithSpaceBetweenThenSortsByAll()
			{
				const string Orderstring = "DoubleValue, IntValue desc";

				var descriptions = _factory.Create<AliasItem>(Orderstring);
                var filter = new ModelFilter<AliasItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(3, sortedItems.OfType<AliasItem>().ElementAt(0).AliasIntValue);
				Assert.AreEqual(1, sortedItems.OfType<AliasItem>().ElementAt(1).AliasIntValue);
				Assert.AreEqual(2, sortedItems.OfType<AliasItem>().ElementAt(2).AliasIntValue);
			}

			[Test]
			public void WhenFilterIsEmptyThenDoesNotSort()
			{
				var descriptions = _factory.Create<AliasItem>(string.Empty);
                var filter = new ModelFilter<AliasItem>(x => true, null, descriptions, 0, -1, false);

				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual(2, sortedItems.OfType<AliasItem>().ElementAt(0).AliasIntValue);
				Assert.AreEqual(1, sortedItems.OfType<AliasItem>().ElementAt(1).AliasIntValue);
				Assert.AreEqual(3, sortedItems.OfType<AliasItem>().ElementAt(2).AliasIntValue);
			}

			[Test]
			public void WhenOrderingByChildPropertyThenUsesChildProperty()
			{
				const string Orderstring = "StringValue/Length desc";

				var descriptions = _factory.Create<AliasItem>(Orderstring);
                var filter = new ModelFilter<AliasItem>(x => true, null, descriptions, 0, -1, false);
				var sortedItems = filter.Filter(_items)
					.ToArray();

				Assert.AreEqual("aaa", sortedItems.OfType<AliasItem>().ElementAt(0).StringValue);
				Assert.AreEqual("aa", sortedItems.OfType<AliasItem>().ElementAt(1).StringValue);
				Assert.AreEqual("a", sortedItems.OfType<AliasItem>().ElementAt(2).StringValue);
			}
		}
	}
}