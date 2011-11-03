using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TauberMatching.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
