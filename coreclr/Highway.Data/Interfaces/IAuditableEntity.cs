
using System;


namespace Highway.Data
{
	/// <summary>
	///     The interface to implement when using with the default Auditable Interceptor to specify an Auditable Entity
	/// </summary>
	public interface IAuditableEntity
	{
		/// <summary>
		///     The date this entity was created
		/// </summary>
		DateTime CreatedDate { get; set; }

		/// <summary>
		///     Who created this entity
		/// </summary>
		string CreatedBy { get; set; }

		/// <summary>
		///     The date this entity was last modified
		/// </summary>
		DateTime ModifiedDate { get; set; }

		/// <summary>
		///     Who last modified this entity
		/// </summary>
		string ModifiedBy { get; set; }
	}
}