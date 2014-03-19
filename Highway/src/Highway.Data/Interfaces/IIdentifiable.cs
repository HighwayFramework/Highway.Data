namespace Highway.Data
{
    using System;

    /// <summary>
    ///     Defines a generalized Id property for identifying an individual entity.
    /// </summary>
    /// <typeparam name="T">The type of the Id.</typeparam>
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
        /// <summary>
        ///     Gets or sets a value identifying the individual entity.
        /// </summary>
        T Id { get; set; }
    }
}