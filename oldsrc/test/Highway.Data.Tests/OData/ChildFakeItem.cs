// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChildFakeItem.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ChildFakeItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Highway.Data.Tests.OData
{
    public class ChildFakeItem
	{
		public IEnumerable<string> Attributes { get; set; } 
	}
}