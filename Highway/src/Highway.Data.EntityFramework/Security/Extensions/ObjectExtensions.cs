using System.Collections.Generic;

namespace Highway.Data.EntityFramework.Security.Extensions
{
    public static class ObjectExtensions
    {
        public static List<DataEntitlementDetail> GetEntitledDetails<T>(this T item)
        {
            return new List<DataEntitlementDetail>();
        }
    }

    public class DataEntitlementDetail
    {
        public string SecuredType { get; set; }
        public Dictionary<string, long?> PermissionsNeeded { get; set; }
    }
}