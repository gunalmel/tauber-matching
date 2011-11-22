using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    //TODO #33,34 See RankStudentsController and its Index View.
    public class RankProjectsController : Controller
    {
        public ActionResult Index(Guid? id)
        {
            return View(StudentService.BuildRankProjectsIndexModelForProject(id));
        }

    }
}
