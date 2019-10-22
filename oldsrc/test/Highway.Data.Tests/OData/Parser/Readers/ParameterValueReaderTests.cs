// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValueReaderTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ParameterValueReaderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Highway.Data.OData.Parser.Readers;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser.Readers
{
    [TestFixture]
	public class ParameterValueReaderTests
	{
		[TestCase("null", typeof(string))]
		[TestCase("'test'", typeof(string))]
		[TestCase("guid'D81D5F0C-2574-4D5C-A394-E280E6E02A7F'", typeof(Guid))]
		[TestCase("guid'926adbb7-328f-44c5-8900-e8f78e4a66f8'", typeof(Guid))]
		[TestCase("guid'D81D5F0C-2574-4D5C-A394-E280E6E02A7F'", typeof(Guid?))]
		[TestCase("guid'926adbb7-328f-44c5-8900-e8f78e4a66f8'", typeof(Guid?))]
		[TestCase("datetime'2012-04-21T12:34:56Z'", typeof(DateTime))]
		[TestCase("datetime'2012-04-21T12:34:56Z'", typeof(DateTime?))]
		[TestCase("null", typeof(DateTime?))]
		[TestCase("1.23M", typeof(decimal))]
		[TestCase("1.23m", typeof(decimal))]
		[TestCase("1.23", typeof(decimal))]
		[TestCase("1.23D", typeof(double))]
		[TestCase("1.23d", typeof(double))]
		[TestCase("1.23", typeof(double))]
		[TestCase("1.23F", typeof(float))]
		[TestCase("1.23f", typeof(float))]
		[TestCase("1.23", typeof(float))]
		[TestCase("123", typeof(long))]
		[TestCase("123", typeof(int))]
		[TestCase("123", typeof(short))]
		[TestCase("123", typeof(long?))]
		[TestCase("null", typeof(long?))]
		[TestCase("123", typeof(int?))]
		[TestCase("null", typeof(int?))]
		[TestCase("123", typeof(short?))]
		[TestCase("null", typeof(short?))]
		[TestCase("123", typeof(ulong))]
		[TestCase("123", typeof(uint))]
		[TestCase("123", typeof(ushort))]
		[TestCase("123", typeof(ulong?))]
		[TestCase("null", typeof(ulong?))]
		[TestCase("123", typeof(uint?))]
		[TestCase("123", typeof(ushort?))]
		[TestCase("null", typeof(ushort?))]
		[TestCase("1.23M", typeof(decimal?))]
		[TestCase("1.23m", typeof(decimal?))]
		[TestCase("1.23", typeof(decimal?))]
		[TestCase("null", typeof(decimal?))]
		[TestCase("1.23D", typeof(double?))]
		[TestCase("1.23d", typeof(double?))]
		[TestCase("1.23", typeof(double?))]
		[TestCase("null", typeof(double?))]
		[TestCase("1.23F", typeof(float?))]
		[TestCase("1.23f", typeof(float?))]
		[TestCase("1.23", typeof(float?))]
		[TestCase("null", typeof(float?))]
		[TestCase("12", typeof(byte))]
		[TestCase("f6", typeof(byte))]
		[TestCase("12", typeof(byte?))]
		[TestCase("f6", typeof(byte?))]
		[TestCase("null", typeof(byte?))]
		[TestCase("true", typeof(bool))]
		[TestCase("false", typeof(bool))]
		[TestCase("1", typeof(bool))]
		[TestCase("0", typeof(bool))]
		[TestCase("true", typeof(bool?))]
		[TestCase("false", typeof(bool?))]
		[TestCase("1", typeof(bool?))]
		[TestCase("0", typeof(bool?))]
		[TestCase("null", typeof(bool?))]
		[TestCase("Highway.Data.Tests.OData.Choice'That'", typeof(Choice))]
		[TestCase("X'ZWFzdXJlLg=='", typeof(byte[]))]
		[TestCase("binary'ZWFzdXJlLg=='", typeof(byte[]))]
		[TestCase("X'ZWFzdXJlLg=='", typeof(Stream))]
		[TestCase("binary'ZWFzdXJlLg=='", typeof(Stream))]
		public void CanConvertValidFilterValue(string token, Type type)
		{
			var reader = new ParameterValueReader(Enumerable.Empty<IValueExpressionFactory>());
			Assert.DoesNotThrow(() => reader.Read(type, token, CultureInfo.CurrentCulture));
		}

		[Test]
		public void DecimalIsNotAssignableFromDouble()
		{
			Assert.False(typeof(decimal).IsAssignableFrom(typeof(double)));
		}

		[Test]
		public void EnumIsAssignableFromAnEnumType()
		{
			Assert.IsTrue(typeof(Enum).IsAssignableFrom(typeof(Choice)));
		}
	}
}
