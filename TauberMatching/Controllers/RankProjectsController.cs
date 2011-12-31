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
    public class RankProjectsController : Controller
    {
        private static string _confMesgReceipents = ConfigurationService.GetEmailConfigParameters().ConfirmationEmailReceivers;

        public ActionResult Index(Guid? id)
        {
            return View(StudentService.GetRankProjectsIndexModelForStudent(id));
        }

        [HttpPost]
        public JsonResult SubmitPreferences(StudentPreferencesDto preferencesDto)
        {
            MatchingDB db = new MatchingDB();
            Student student = db.Students.Include("Matchings.Project").Include("StudentFeedbacks.Project").Where(s => s.Id == preferencesDto.StudentId && s.Guid == new Guid(preferencesDto.StudentGuid)).FirstOrDefault();

            string message = student == null ? "Authentication failure: Student can not be identified." : "Success";
            var jsonResult = new JsonResult();
            jsonResult.Data = message;

            if (student == null)
                return jsonResult;

            // Update the project scores with the scores coming from UI
            foreach (StudentScoreDto sSDto in preferencesDto.StudentPreferences)
            {
                student.Matchings.Where(m => m.Project.Id == sSDto.ProjectId).FirstOrDefault().StudentScore = sSDto.Score;
            }
            // Remove all feedbacks 
            ICollection<StudentFeedback> studentFeedbacks = student.StudentFeedbacks.ToList();
            student.StudentFeedbacks.Clear();
            foreach (StudentFeedback feedback in studentFeedbacks)
            {
                db.StudentFeedbacks.Remove(feedback);
            }

            // Add feedbacks that came from UI.
            IList<StudentFeedback> feedbacks = new List<StudentFeedback>();
            if (preferencesDto.StudentFeedback != null)
            {
                foreach (StudentFeedbackDto sfDto in preferencesDto.StudentFeedback)
                {
                    // TODO If a project is assigned multiple feedback scores find and clean it here.
                    StudentFeedback sf = new StudentFeedback() { Student=student, Type = sfDto.Type };
                    sf.Project = db.Projects.Where(p => p.Id == sfDto.ProjectId).FirstOrDefault();
                    sf.FeedbackScore = Int16.Parse(sfDto.FeedbackScore);
                    feedbacks.Add(sf);
                }
            }
            student.StudentFeedbacks = feedbacks;
            student.OtherComments = preferencesDto.OtherComments;
            student.ScoreDate = DateTime.Now;
            db.SaveChanges();
            db.Dispose();
            SendConfirmationMessage(student);
            return jsonResult;
        }

        private string GetRankingConfirmationText(Student s)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var group in s.Matchings.GroupBy(m => m.StudentScore))
            {
                builder.Append("<b>"+group.Key + ": </b>");
                foreach (Matching m in group)
                    builder.Append(m.Project.Name + ", ");
                builder.Remove((builder.Length - 2), 2);
                builder.Append("<br/>");
            }
            builder.Append("<br/>");

            var positiveFeedback = s.StudentFeedbacks.Where(f=>f.Type=="Positive").OrderBy(f=>f.FeedbackScore);
            var constructiveFeedback = s.StudentFeedbacks.Where(f => f.Type == "Constructive").OrderBy(f => f.FeedbackScore);
            if (positiveFeedback != null && positiveFeedback.Count() > 0)
            {
                builder.Append("<b>Positive Feedback: </b>");
                foreach (var sf in positiveFeedback)
                {
                    builder.Append("<b>"+sf.FeedbackScore.ToString() + ".</b> " + sf.Project.Name + ", ");
                }
                builder.Remove((builder.Length - 2), 2);
                builder.Append("<br/>");
            }
            if (constructiveFeedback != null && constructiveFeedback.Count() > 0)
            {
                builder.Append("<b>Constructive Feedback: </b>");
                foreach (var sf in constructiveFeedback)
                {
                    builder.Append("<b>" + sf.FeedbackScore.ToString() + ".</b> " + sf.Project.Name + ", ");
                }
                builder.Remove((builder.Length - 2), 2);
                builder.Append("<br/>");
            }
            if (s.OtherComments != null && s.OtherComments != "" && s.OtherComments.Length > 0)
                builder.Append("<b>Your Comments: </b>"+s.OtherComments+"<br/>");
            builder.Append("<br/>");
            return builder.ToString();
        }
        private void SendConfirmationMessage(Student s)
        {
            Contact c = new Contact(s);
            EmailQueueMessage eqm= new EmailQueueMessage(c,EmailType.StudentSubmit,GetRankingConfirmationText(s),c.Email+","+_confMesgReceipents);
            EmailQueueService.QueueMessage(eqm);
        }
    }
}
