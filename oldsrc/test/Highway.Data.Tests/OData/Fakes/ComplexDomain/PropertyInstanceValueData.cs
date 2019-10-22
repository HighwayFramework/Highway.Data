// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInstanceValueData.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   A property instance value data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Highway.Data.Tests.OData.Fakes.ComplexDomain
{
    /// <summary>
	/// A property instance value data.
	/// </summary>
	public class PropertyInstanceValueData
	{
		/// <summary>
		/// The value type.
		/// </summary>
		public ValueTypeDefinitionData? ValueType { get; set; }

		/// <summary>
		/// The DateTime value.
		/// </summary>
		public DateTime? DateTimeValue { get; set; }

		/// <summary>
		/// The reference user defined id value.
		/// </summary>
		public string ReferenceValue { get; set; }

		/// <summary>
		/// The bool value.
		/// </summary>
		public bool? BoolValue { get; set; }

		/// <summary>
		/// The float value. 
		/// </summary>
		public float? FloatValue { get; set; }

		/// <summary>
		/// The int value.
		/// </summary>
		public int? IntValue { get; set; }

		/// <summary>
		/// The string value .
		/// </summary>
		public string StringNonUnicodeValue { get; set; }

		/// <summary>
		/// Overriden. Determines whether the specified Object is equal to the current object.
		/// </summary>
		/// <param name="other">The Object to compare with the current object.</param>
		/// <returns>Returns true if the specified object is equal to the current object; otherwise false.</returns>
		public override bool Equals(object other)
		{
			return Equals(other as PropertyInstanceValueData);
		}

		/// <summary>
		/// Determines whether the specified PropertyInstanceValueData is equal to the current PropertyInstanceValueData.
		/// </summary>
		/// <param name="that">The PropertyInstanceValueData to compare with the current object.</param>
		/// <returns>Returns true if the specified object has the same value type and same vale as the current PropertyInstanceValueData; otherwise false.</returns>
		public virtual bool Equals(PropertyInstanceValueData that)
		{
			// if that is null - not equal
			if (ReferenceEquals(that, null))
			{
				return false;
			}

			// if that is has same reference to me - equal
			if (ReferenceEquals(this, that))
			{
				return true;
			}

			// if that has a different value type than me - not equal
			if (ValueType != that.ValueType)
			{
				return false;
			}

			// finally - if that has the same actual value as me - equal (otherwise, not equal)
			switch (ValueType)
			{
				case ValueTypeDefinitionData.Reference:
					return ReferenceValue.Equals(that.ReferenceValue);

				case ValueTypeDefinitionData.DateTime:
					return DateTimeValue.Equals(that.DateTimeValue);

				case ValueTypeDefinitionData.Bool:
					return BoolValue == that.BoolValue;

				case ValueTypeDefinitionData.Float:
					return FloatValue == that.FloatValue;

				case ValueTypeDefinitionData.Int:
					return IntValue == that.IntValue;

				case ValueTypeDefinitionData.StringNonUnicode:
					return StringNonUnicodeValue.Equals(that.StringNonUnicodeValue);
				default:
					return false;
			}
		}

		/// <summary>
		/// Overriden. Returns a hash code for the current Object.
		/// </summary>
		/// <returns>A hash code for the current Object.</returns>
		public override int GetHashCode()
		{
			switch (ValueType)
			{
				case ValueTypeDefinitionData.Reference:
					return ReferenceValue.GetHashCode();

				case ValueTypeDefinitionData.DateTime:
					return DateTimeValue.GetHashCode();

				case ValueTypeDefinitionData.Bool:
					return BoolValue.GetHashCode();

				case ValueTypeDefinitionData.Float:
					return FloatValue.GetHashCode();

				case ValueTypeDefinitionData.Int:
					return IntValue.GetHashCode();

				case ValueTypeDefinitionData.StringNonUnicode:
					return StringNonUnicodeValue.GetHashCode();

				default:
					return base.GetHashCode();
			}
		}
	}
}