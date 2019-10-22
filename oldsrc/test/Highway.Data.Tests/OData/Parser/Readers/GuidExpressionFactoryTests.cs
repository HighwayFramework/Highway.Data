// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the GuidExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class GuidExpressionFactoryTests
	{
		private GuidExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new GuidExpressionFactory();
		}

		[Test]
		public void WhenFilterIncludesGuidParameterInDoubleQuotesThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid\"{0}\"", guid);

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesGuidParameterThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid'{0}'", guid);

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesGuidParameterWithNoDashesInDoubleQuotesThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid\"{0}\"", guid.ToString("N"));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesGuidParameterWithNoDashesThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid'{0}'", guid.ToString("N"));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}
