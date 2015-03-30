// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NorthwindCustomer.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the NorthwindCustomer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Highway.Data.Tests.OData.Fakes
{
	public class NorthwindCustomer
	{
		public string CustomerID { get; set; }

		public string CompanyName { get; set; }

		public string ContactName { get; set; }

		public string ContactTitle { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string Region { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public string Phone { get; set; }
	}
}