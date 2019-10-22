// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInstanceData.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   A property instance data .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Highway.Data.Tests.OData.Fakes.ComplexDomain
{
    /// <summary>
	/// A property instance data .
	/// </summary>
	public class PropertyInstanceData
	{
		/// <summary>
		/// The property definition name that this instance belongs to.
		/// </summary>
		public string DefinitionName { get; set; }

		/// <summary>
		/// The values for this instance.
		/// </summary>
		public PropertyInstanceValueData[] Values { get; set; }

		/// <summary>
		/// Overriden. Determines whether the specified Object is equal to the current object.
		/// </summary>
		/// <param name="other">The Object to compare with the current object.</param>
		/// <returns>Returns true if the specified object is equal to the current object; otherwise false.</returns>
		public override bool Equals(object other)
		{
			return Equals(other as PropertyInstanceData);
		}

		/// <summary>
		/// Determines whether the specified PropertyInstanceData is equal to the current PropertyInstanceData.
		/// </summary>
		/// <param name="that">The PropertyInstanceData to compare with the current object.</param>
		/// <returns>Returns true if the specified object has the same DefinitionName and the same values 
		/// (using the PropertyInstanceValueData Equals method) as the current PropertyInstanceValueData; 
		/// otherwise false.</returns>
		public virtual bool Equals(PropertyInstanceData that)
		{
			// if data is null - not equal
			if (ReferenceEquals(that, null))
			{
				return false;
			}

			// if data is has same reference to me - equal
			if (ReferenceEquals(this, that))
			{
				return true;
			}

			// otherwise do comparison logic here

			// First check it's the same definition name
			if (!DefinitionName.Equals(that.DefinitionName))
			{
				return false;
			}

			// Second, check it's the same number of values
			if (Values.Length != that.Values.Length)
			{
				return false;
			}

			// Last check it's the same exact values  
			return !Values.Except(that.Values).Any();
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return DefinitionName.GetHashCode();
		}
	}
}