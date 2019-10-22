// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the DateTimeExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class DateTimeExpressionFactoryTests
	{
		private DateTimeExpressionFactory _factory;
		private DateTime _dateTime;

		[SetUp]
		public void Setup()
		{
			_factory = new DateTimeExpressionFactory();
			_dateTime = new DateTime(2012, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterInDoubleQuotesThenReturnedExpressionContainsDateTime()
		{
			var parameter = string.Format("datetime\"{0}\"", _dateTime.ToString("yyyy-MM-ddThh:mm:ss"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(_dateTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterThenReturnedExpressionContainsDateTime()
		{
			var parameter = string.Format("datetime'{0}'", _dateTime.ToString("yyyy-MM-ddThh:mm:ss"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(_dateTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterWithMillisecondsThenReturnedExpressionContainsDateTime()
		{
			_dateTime = new DateTime(2012, 1, 1, 12, 0, 0, 11, DateTimeKind.Utc);
			var parameter = string.Format("datetime'{0}'", _dateTime.ToString("o"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(_dateTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterWithZuluInDoubleQuotesThenReturnedExpressionContainsUtcDateTime()
		{
			var utcTime = _dateTime.ToUniversalTime();
			var parameter = string.Format("datetime\"{0}\"", utcTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

			var expression = _factory.Convert(parameter);
			
			Assert.AreEqual(utcTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterWithZuluThenReturnedExpressionContainsUtcDateTime()
		{
			var utcTime = _dateTime.ToUniversalTime();
			var parameter = string.Format("datetime'{0}'", utcTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(utcTime, expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}