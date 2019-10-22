// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AliasItem.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the AliasItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Highway.Data.Tests.OData
{
    [DataContract]
	public class AliasItem
	{
		private readonly Collection<FakeChildItem> _children = new Collection<FakeChildItem>();
		
		[DataMember(Name = "Text")]
		private string _stringValue;

		[DataMember(Name = "ID")]
		public int AliasID { get; set; }

		[DataMember(Name = "GlobalID")]
		public Guid AliasGlobalID { get; set; }

		[DataMember(Name = "IntValue")]
		[XmlElement(ElementName = "Number")]
		public int AliasIntValue { get; set; }

		[DataMember(Name = "DoubleValue")]
		public double AliasDoubleValue { get; set; }

		[DataMember(Name = "DecimalValue")]
		public decimal AliasDecimalValue { get; set; }

		public string StringValue
		{
			get
			{
				return _stringValue;
			}

			set
			{
				_stringValue = value;
			}
		}

		[DataMember(Name = "DateValue")]
		public DateTime AliasDateValue { get; set; }

		[DataMember(Name = "Duration")]
		public TimeSpan AliasDuration { get; set; }

		[DataMember(Name = "PointInTime")]
		public DateTimeOffset AliasPointInTime { get; set; }

		[DataMember(Name = "Choice")]
		public Choice ChoiceValue { get; set; }

		[DataMember(Name = "Child")]
		public ChildFakeItem AliasChild { get; set; }

		[DataMember(Name = "Children")]
		public ICollection<FakeChildItem> AliasChildren
		{
			get { return _children; }
		}
	}
}