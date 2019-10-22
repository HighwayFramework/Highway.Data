// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeOffsetExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the DateTimeOffsetExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Xml;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class DateTimeOffsetExpressionFactoryTests
	{
		private DateTimeOffsetExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new DateTimeOffsetExpressionFactory();
		}

		[Test]
		public void WhenFilterIncludesDateTimeOffsetParameterInDoubleQuotesThenReturnedExpressionContainsDateTimeOffset()
		{
			var dateTimeOffset = new DateTimeOffset(2012, 5, 6, 18, 10, 0, 100, TimeSpan.FromHours(2));
			var parameter = string.Format("datetimeoffset\"{0}\"", XmlConvert.ToString(dateTimeOffset));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<DateTimeOffset>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeOffsetParameterThenReturnedExpressionContainsDateTimeOffset()
		{
			var dateTimeOffset = new DateTimeOffset(2012, 5, 6, 18, 10, 0, 100, TimeSpan.FromHours(2));
			var parameter = string.Format("datetimeoffset'{0}'", XmlConvert.ToString(dateTimeOffset));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<DateTimeOffset>(expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}