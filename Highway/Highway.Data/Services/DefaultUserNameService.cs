using Highway.Data.Interfaces;

namespace Highway.Data.Services
{
    /// <summary>
    /// Stub implementation of the User Name service to supply example names for Auditable interceptor.
    /// </summary>
    public class DefaultUserNameService : IUserNameService
    {
        public string GetCurrentUserName()
        {
            return "Default User";
        }
    }
}