// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShortExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ShortExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class ShortExpressionFactoryTests
	{
		private ShortExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new ShortExpressionFactory();
		}

		[Test]
		public void WhenFilterIncludesShortParameterThenReturnedExpressionContainsShort()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<short>(expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}