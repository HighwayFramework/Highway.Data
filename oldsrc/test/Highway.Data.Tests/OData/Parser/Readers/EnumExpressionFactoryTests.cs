// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the EnumExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class EnumExpressionFactoryTests
	{
		private const string EnumString = "Highway.Data.Tests.OData.Choice'That'";
		private EnumExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new EnumExpressionFactory();
		}

		[Test]
		public void WhenFilterIncludesCorrectEnumValueThenReturnedExpressionContainsEnumValue()
		{
			var expression = _factory.Convert(EnumString);

			Assert.IsAssignableFrom<Choice>(expression.Value);
		}

        [Test]
        public void WhenFilterIncludesNumberForEnumValueThenReturnExpressionHoldsThatNumber()
        {
            var expression = _factory.Convert("1");

            Assert.IsAssignableFrom<string>(expression.Value);
        }
	}
}