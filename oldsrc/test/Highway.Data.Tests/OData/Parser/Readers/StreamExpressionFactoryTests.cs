// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the StreamExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class StreamExpressionFactoryTests
	{
		private const string Base64 = "TWFuIGlzIG/pc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=";
		private StreamExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new StreamExpressionFactory();
		}

		[Test]
		public void WhenFilterIncludesBinaryParameterWithPrefixBinaryThenReturnedExpressionContainsStream()
		{
			var expression = _factory.Convert(string.Format("binary'{0}'", Base64));

			Assert.IsAssignableFrom<MemoryStream>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesBinaryParameterWithPrefixXThenReturnedExpressionContainsStream()
		{
			var expression = _factory.Convert(string.Format("X'{0}'", Base64));

			Assert.IsAssignableFrom<MemoryStream>(expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}