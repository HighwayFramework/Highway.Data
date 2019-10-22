// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the BooleanExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class BooleanExpressionFactoryTests
	{
		private BooleanExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new BooleanExpressionFactory();
		}

		[TestCase("1", true)]
		[TestCase("0", false)]
		[TestCase("2", null)]
		public void WhenFilterIncludesBooleanParameterAsNumberThenReturnedExpressionContainsBoolean(string parameter, object value)
		{
			var expression = _factory.Convert(parameter);

			Assert.AreEqual(value, expression.Value);
		}

		[TestCase("true", true)]
		[TestCase("True", true)]
		[TestCase("false", false)]
		[TestCase("False", false)]
		[TestCase("blah", null)]
		public void WhenFilterIncludesBooleanParameterAsWordThenReturnedExpressionContainsBoolean(string parameter, object value)
		{
			var expression = _factory.Convert(parameter);

			Assert.AreEqual(value, expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenReturnsNullValue()
		{
			const string Parameter = "blah";

			Assert.AreEqual(null, _factory.Convert(Parameter).Value);
		}
	}
}