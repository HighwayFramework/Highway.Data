#region

using System.Web.Mvc;

#endregion

namespace Highway.Data.Rest.ExampleAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}