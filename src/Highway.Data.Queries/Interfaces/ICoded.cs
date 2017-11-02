namespace Highway.Data.Queries
{
	using System;
	/// <summary>
	///		Defines a generalized Code property for uniquely identifying an individual entity.
	/// </summary>
	public interface ICoded
	{
		string Code { get; set; }
	}
}