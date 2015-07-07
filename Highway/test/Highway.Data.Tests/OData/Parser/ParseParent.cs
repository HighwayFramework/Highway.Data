// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseParent.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ParseParent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Highway.Data.Tests.OData.Parser
{
	public class ParseParent
	{
		public ParseObject Item { get; set; }

		public int Number { get; set; }

		public class ParseObject
		{
			public int Value { get; set; }

			public static ParseObject Parse(string input)
			{
				var value = int.Parse(input);
				return new ParseObject { Value = value };
			}

			public override string ToString()
			{
				return Value.ToString();
			}
		}
	}
}