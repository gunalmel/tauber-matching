using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    public class RankProjectsController : Controller
    {
        public ActionResult Index(Guid? id)
        {
            return View(StudentService.GetRankProjectsIndexModelForStudent(id));
        }

        [HttpPost]
        public JsonResult SubmitPreferences(StudentPreferencesDto preferencesDto)
        {
            MatchingDB db = new MatchingDB();
            Student project = db.Students.Include("Matchings.Project").Include("StudentFeedbacks.Project").Where(s => s.Id == preferencesDto.StudentId && s.Guid == new Guid(preferencesDto.StudentGuid)).FirstOrDefault();

            string message = project == null ? "Authentication failure: Student can not be identified." : "Success";
            var jsonResult = new JsonResult();
            jsonResult.Data = message;

            if (project == null)
                return jsonResult;

            return jsonResult;
        }
    }
}
