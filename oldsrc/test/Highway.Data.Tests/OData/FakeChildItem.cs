// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeChildItem.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the FakeChildItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Highway.Data.Tests.OData
{
    [DataContract]
	public class FakeChildItem
	{
		private readonly Collection<FakeGrandChildItem> _children = new Collection<FakeGrandChildItem>();

		public int ID { get; set; }

		public string ChildStringValue { get; set; }

		public ICollection<FakeGrandChildItem> Children
		{
			get { return _children; }
		}
	}
}