// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeInstanceData.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   A type instance data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Highway.Data.Tests.OData.Fakes.ComplexDomain
{
    /// <summary>
	/// A type instance data.
	/// </summary>
	public class TypeInstanceData
	{
		/// <summary>
		/// The unique id of this object, given by the user who created it.
		/// </summary>
		public string UserDefinedId { get; set; }

		/// <summary>
		/// The last changed time.
		/// </summary>
		public DateTime LastChangedDate { get; set; }

		/// <summary>
		/// The full name of the type definition that this instance belongs to.
		/// Full name includes all hierarchies (for Example: CPU\SNB\SNB2Core1Gfx).
		/// </summary>
		public string DefinitionFullName { get; set; }

		/// <summary>
		/// The properties that belong to this type instance.
		/// </summary>
		public PropertyInstanceData[] Properties { get; set; }

		/// <summary>
		/// Determines whether this type instance was soft deleted.
		/// </summary>
		public bool IsDeleted { get; set; }

		/// <summary>
		/// The name of the user which last updated this object.
		/// </summary>
		public string LastUpdatedBy { get; set; }
	}
}