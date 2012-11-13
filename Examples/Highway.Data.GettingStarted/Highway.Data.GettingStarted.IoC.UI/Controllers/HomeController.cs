using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Highway.Data.GettingStarted.DataAccess.Queries;
using Highway.Data.GettingStarted.Domain.Entities;
using Highway.Data.Interfaces;

namespace Highway.Data.GettingStarted.IoC.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            IEnumerable<Person> result = _repository.Find(new FindPersonByLastName("Liles"));
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
