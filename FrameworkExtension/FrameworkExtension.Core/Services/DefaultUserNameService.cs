using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.Services
{
    public class DefaultUserNameService : IUserNameService
    {
        public string GetCurrentUserName()
        {
            return "Default User";
        }
    }
}