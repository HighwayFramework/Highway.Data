// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the SelectExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Highway.Data.OData;
using Highway.Data.OData.Parser;
using NUnit.Framework;

namespace Highway.Data.Tests.OData
{
    public class SelectExpressionFactoryTests
	{
		private SelectExpressionFactory<FakeItem> _factory;
		private FakeItem[] _items;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			var memberNameResolver = new MemberNameResolver();
			_factory = new SelectExpressionFactory<FakeItem>(memberNameResolver, new RuntimeTypeProvider(memberNameResolver));

			_items = new[]
				{
					new FakeItem { IntValue = 2, DoubleValue = 5 }, 
					new FakeItem { IntValue = 1, DoubleValue = 4 }, 
					new FakeItem { IntValue = 3, DoubleValue = 4 }
				};
		}

		[Test]
		public void WhenApplyingSelectionThenReturnsObjectWithOnlySelectedPropertiesAsFields()
		{
			var expression = _factory.Create("Number").Compile();

			var selection = _items.Select(expression);

			Assert.True(selection.All(x => x.GetType().GetProperty("Number") != null));
		}
	}
}