// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenSetTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TokenSetTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Highway.Data.OData.Parser;
using NUnit.Framework;

namespace Highway.Data.Tests.OData.Parser
{
    [TestFixture]
	public class TokenSetTests
	{
		[Test]
		public void FunctionTokenSetToStringWritesOperationLeftRight()
		{
			var set = new FunctionTokenSet { Left = "Left", Operation = "Operation", Right = "Right" };

			Assert.AreEqual("Operation Left Right", set.ToString());
		}

		[Test]
		public void TokenSetToStringWritesLeftOperationRight()
		{
			var set = new TokenSet { Left = "Left", Operation = "Operation", Right = "Right" };

			Assert.AreEqual("Left Operation Right", set.ToString());
		}
	}
}
