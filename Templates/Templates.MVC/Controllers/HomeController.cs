using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Templates.BaseTypes;
using Highway.Data.Interfaces;
using Templates.Models;

namespace Templates.Controllers
{
    public class HomeController : BaseController
    {
        private IRepository repo;

        public HomeController(IRepository repo)
        {
            this.repo = repo;
        }

        public ActionResult Index()
        {
            Logger.Debug("Home Controller, checking in sir.");
            return View(repo.Context.AsQueryable<DeleteMe>().ToList());
        }

    }
}
