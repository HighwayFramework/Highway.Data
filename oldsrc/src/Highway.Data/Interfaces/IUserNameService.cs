namespace Highway.Data
{
    /// <summary>
    ///     This gives a standard interface for the user name service,
    ///     Which is leveraged by the BeforeSave interceptors to supply user names for audit tagging on entities.
    /// </summary>
    public interface IUserNameService
    {
        /// <summary>
        ///     Basic Method for returning the current user name
        /// </summary>
        /// <returns>The current user's name or login</returns>
        string GetCurrentUserName();
    }
}