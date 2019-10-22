// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnonymousTypeSerializerHelper.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the AnonymousTypeSerializerHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

namespace Highway.Data.Tests.OData
{
    internal static class AnonymousTypeSerializerHelper
	{
		public static readonly MethodInfo InnerChangeTypeMethod = typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(Type) });
	}
}