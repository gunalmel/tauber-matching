using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    public class RankStudentsController : Controller
    {
        public ActionResult Index(Guid? id)
        {
            return View(ProjectService.GetRankStudentsIndexModelForProject(id));
        }

        [HttpPost]
        public JsonResult SubmitPreferences(ProjectPreferencesDto preferencesDto)
        {
            MatchingDB db = new MatchingDB();
            Project project = db.Projects.Include("Matchings.Student").Include("ProjectRejects.Student").Where(p => p.Id == preferencesDto.ProjectId && p.Guid == new Guid(preferencesDto.ProjectGuid)).FirstOrDefault();
            
            string message = project == null ? "Authentication failure: Project can not be identified." : "Success";
            var jsonResult = new JsonResult();
            jsonResult.Data = message;

            if (project == null)
                return jsonResult;
            
            // Update the project scores with the scores coming from UI
            foreach (ProjectScoreDto pSDto in preferencesDto.ProjectPreferences)
            {
                project.Matchings.Where(m => m.Student.Id == pSDto.StudentId).FirstOrDefault().ProjectScore = pSDto.Score;
            }
            // Remove all rejects 
            ICollection<ProjectReject> projectRejects = project.ProjectRejects.ToList();
            project.ProjectRejects.Clear();
            foreach (ProjectReject reject in projectRejects)
            {
                db.ProjectRejects.Remove(reject);
            }
            
            // Add rejects that came from UI.
            IList<ProjectReject> userRejects = new List<ProjectReject>();
            if (preferencesDto.ProjectRejects != null)
            {
                foreach (ProjectRejectDto pRDto in preferencesDto.ProjectRejects)
                {
                    ProjectReject pr = new ProjectReject();
                    pr.Student = db.Students.Where(s => s.Id == pRDto.StudentId).FirstOrDefault();
                    pr.Reason = pRDto.Reason;
                    userRejects.Add(pr);
                }
            }
            project.ProjectRejects = userRejects;
            project.Feedback = preferencesDto.Feedback;
            project.ScoreDate = DateTime.Now;
            db.SaveChanges();
            db.Dispose();
            return jsonResult;
        }
    }
}
