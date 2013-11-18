#region

using System.Web.Mvc;

#endregion

namespace Highway.Data.Rest.ExampleAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}