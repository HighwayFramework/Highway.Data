// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueTypeDefinitionData.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines a value type in Metal.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Highway.Data.Tests.OData.Fakes.ComplexDomain
{
	/// <summary>
	/// Defines a value type in Metal.
	/// </summary>
	public enum ValueTypeDefinitionData : short
	{
		Reference = 0, // Reference to another TypeDefinitionData
		DateTime = 1, 
		Bool = 2, 
		Float = 3, 
		Int = 4, 
		StringNonUnicode = 5
	}
}