// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterParserTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ParameterParserTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Linq;
using Highway.Data.OData;
using Highway.Data.OData.Parser;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser
{
    [TestFixture]
	public class ParameterParserTests
	{
		private ParameterParser<FakeItem> _parser;
		private FakeItem[] _items;
		private FakeItem[] _nestedItems;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			_parser = new ParameterParser<FakeItem>();

			_items = new[]
				{
					new FakeItem { IntValue = 2, DoubleValue = 2 }, 
					new FakeItem { IntValue = 1, DoubleValue = 1 }, 
					new FakeItem { IntValue = 3, DoubleValue = 3 }
				};

			_nestedItems = new[]
				{
					new FakeItem
						{
							IntValue = 2, 
							DoubleValue = 2, 
							Children =
								{
									new FakeChildItem { ChildStringValue = "1" }, 
									new FakeChildItem { ChildStringValue = "2" }, 
									new FakeChildItem { ChildStringValue = "3" }
								}
						}, 
					new FakeItem
						{
							IntValue = 1, 
							DoubleValue = 1, 
							Children =
								{
									new FakeChildItem { ChildStringValue = "2" }, 
									new FakeChildItem { ChildStringValue = "3" }, 
									new FakeChildItem { ChildStringValue = "4" }
								}
						}, 
					new FakeItem
						{
							IntValue = 3, 
							DoubleValue = 3, 
							Children =
								{
									new FakeChildItem { ChildStringValue = "3" }, 
									new FakeChildItem { ChildStringValue = "4" }, 
									new FakeChildItem { ChildStringValue = "5" }
								}
						}, 
				};
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsAnyFilterParameterThenReturnedModelFilterFilteringCollectionByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "Children/any(a: a/ChildStringValue eq '1')" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection, _nestedItems);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterParameterAndApplyingAsExtensionMethodThenReturnedModelFilterFilteringByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "IntValue eq 1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterParameterAndSortThenReturnedModelFilterFilteringAndSortedByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "IntValue ge 1" }, { "$orderby", "IntValue desc" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(3, filteredItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(2, filteredItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(1, filteredItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterParameterThenReturnedModelFilterFilteringByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "IntValue eq 1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterSortSkipAndTopThenReturnedModelFilterFilteringFindsItem(bool useModelFilter)
		{
			var collection = new NameValueCollection
							 {
								 { "$filter", "IntValue ge 1" }, 
								 { "$skip", "1" }, 
								 { "$top", "1" }, 
								 { "$orderby", "IntValue desc" }
							 };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(2, filteredItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(1, filteredItems.Length);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsMathFunctionFilterParameterThenReturnedModelFilterFilteringByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "floor(DoubleValue) gt 1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(2, filteredItems.Count());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsNoSystemParametersThenReturnedModelFilterWithoutFiltering(bool useModelFilter)
		{
			var collection = new NameValueCollection();
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(3, filteredItems.Count());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsSkipParameterThenReturnedModelFilterSkippingItems(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$skip", "2" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsTopParameterThenReturnedModelFilterWithTopItems(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$top", "1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		private object[] GetFilteredItems(bool useModelFilter, NameValueCollection collection)
		{
			return GetFilteredItems(useModelFilter, collection, _items);
		}

		private object[] GetFilteredItems(bool useModelFilter, NameValueCollection collection, FakeItem[] items)
		{
			var filteredItems = useModelFilter
				? GetModelFilter(collection).Filter(items)
				: items.Filter(collection);
			return filteredItems.ToArray();
		}

		private IModelFilter<FakeItem> GetModelFilter(NameValueCollection parameters)
		{
			var filter = _parser.Parse(parameters);
			return filter;
		}
	}
}
