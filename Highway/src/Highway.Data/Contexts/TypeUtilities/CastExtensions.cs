using System.Reflection;

namespace Highway.Data.Contexts.TypeUtilities
{
    public class CastExtensions
    {
        public static T Cast<T>(object o)
        {
            return (T)o;
        }
    }
}