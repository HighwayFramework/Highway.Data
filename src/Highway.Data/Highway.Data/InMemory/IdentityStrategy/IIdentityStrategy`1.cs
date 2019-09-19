namespace Highway.Data.Contexts
{
    /// <summary>
    /// Provides an interface by which Identity values can be assigned.
    /// </summary>
    /// <typeparam name="T">The type of the </typeparam>
    public interface IIdentityStrategy<in T>
        where T : class
    {
        /// <summary>
        /// Assigns an identity value to the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity"></param>
        void Assign(T entity);
    }
}