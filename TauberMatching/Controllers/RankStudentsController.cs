using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;
using System.Text;

namespace TauberMatching.Controllers
{
    public class RankStudentsController : Controller
    {
        private static string _confMesgReceipents = ConfigurationService.GetEmailConfigParameters().ConfirmationEmailReceivers;
        private static Dictionary<string, string> _scoreDictionary = new Dictionary<string, string>();

        static RankStudentsController(){
            MatchingDB db = new MatchingDB();
            _scoreDictionary= db.ScoreDetails.Where(s => s.ScoreFor == "Project").ToDictionary(k => k.Score, v => v.ScoreTypeDisplay);
        }

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
            SendConfirmationMessage(project);
            return jsonResult;
        }

        private string GetRankingConfirmationText(Project p)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var group in p.Matchings.Where(m=>m.ProjectScore!=ProjectScore.Reject.ToString()).OrderBy(m=>m.ProjectScore).GroupBy(m => m.ProjectScore))
            {
                builder.Append("<b>" + _scoreDictionary[group.Key] + ": </b>");
                foreach (Matching m in group)
                    builder.Append(m.Student.FullName + ", ");
                builder.Remove((builder.Length - 2), 2);
                builder.Append("<br/>");
            }
            builder.Append("<br/><br/>");
            foreach (var reject in p.ProjectRejects)
            {
                builder.Append("<b>"+reject.Student.FullName+" is rejected. Reason is: </b>"+reject.Reason+"<br/>");
            }
            if (p.Feedback != null && p.Feedback != "" && p.Feedback.Length > 0)
                builder.Append("<b>Your Comments: </b>" + p.Feedback + "<br/>");
            builder.Append("<br/>");
            return builder.ToString();
        }
        private void SendConfirmationMessage(Project p)
        {
            Contact c = new Contact(p);
            EmailQueueMessage eqm = new EmailQueueMessage(c, EmailType.ProjectSubmit, GetRankingConfirmationText(p), c.Email + "," + _confMesgReceipents);
            EmailQueueService.QueueMessage(eqm);
        }
    }
}
