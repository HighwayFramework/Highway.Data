// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Choice.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the Choice type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Highway.Data.Tests.OData
{
    [Flags]
	public enum Choice
	{
		This = 1, 
		That = 2, 
		Either = This | That
	}
}