#region

using System.Web.Helpers;

#endregion

namespace Highway.Data.Rest.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object item)
        {
            return Json.Encode(item);
        }

        public static T FromJson<T>(this string json)
        {
            return Json.Decode<T>(json);
        }
    }
}