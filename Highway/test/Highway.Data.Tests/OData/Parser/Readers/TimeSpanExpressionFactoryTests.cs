// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TimeSpanExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Xml;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class TimeSpanExpressionFactoryTests
	{
		private TimeSpanExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new TimeSpanExpressionFactory();
		}

		[Test]
		public void WhenFilterIncludesTimeSpanParameterInDoubleQuotesThenReturnedExpressionContainsTimeSpan()
		{
			var timeSpan = new TimeSpan(1, 2, 15, 00);
			var parameter = string.Format("time\"{0}\"", XmlConvert.ToString(timeSpan));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<TimeSpan>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesTimeSpanParameterThenReturnedExpressionContainsTimeSpan()
		{
			var timeSpan = new TimeSpan(1, 2, 15, 00);
			var parameter = string.Format("time'{0}'", XmlConvert.ToString(timeSpan));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<TimeSpan>(expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}