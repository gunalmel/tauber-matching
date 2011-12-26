using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    public class ReportsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GenerateRankingExcelReport()
        {
            string[] headers = new string[4] { "Project","Student","Project Score","Student Score"};
            string[] rowKeys = new string[4] { "p", "s", "ps", "ss"};
            IList rows;
            using(MatchingDB db = new MatchingDB())
            {
            rows = (from m in db.Matchings.OrderBy(ma=>ma.Project.Name)
                           select new
                           {
                               p = m.Project.Name,
                               s = m.Student.UniqueName,
                               ps = m.ProjectScore,
                               ss = m.StudentScore
                           }).ToList();
        }
            var result= this.Excel("Rankings.xlsx", "Rankings", rows, headers, rowKeys);
            return result;
        }

        [HttpGet]
        public ActionResult GenerateStudentFeedbackExcelReport()
        {
            string[] headers = new string[4] { "Student", "Project", "Feedback Type", "Feedback Score" };
            string[] rowKeys = new string[4] { "s", "p", "ft", "fs" };
            IList rows;
            using (MatchingDB db = new MatchingDB())
            {
                rows = (from sf in db.StudentFeedbacks.OrderBy(f=>f.Student.FirstName).ThenBy(f=>f.Student.LastName)
                        select new
                        {
                            s = sf.Student.FirstName+" "+sf.Student.LastName,
                            p = sf.Project.Name,
                            ft = sf.Type,
                            fs = sf.FeedbackScore
                        }).ToList();
            }
            var result = this.Excel("StudentFeedback.xlsx", "Student Feedbacks", rows, headers, rowKeys);
            return result;
        }

        [HttpGet]
        public ActionResult GenerateProjectRejectsExcelReport()
        {
            string[] headers = new string[4] { "Project", "Student", "Degree","Reason" };
            string[] rowKeys = new string[4] { "p", "s", "d", "r" };
            IList rows;
            using (MatchingDB db = new MatchingDB())
            {
                rows = (from p in db.Projects from pr in p.ProjectRejects
                        select new
                        {
                            p = p.Name,
                            s = pr.Student.FirstName+" "+pr.Student.LastName,
                            d = pr.Student.Degree,
                            r = pr.Reason
                        }).ToList();
            }
            var result = this.Excel("ProjectReject.xlsx", "ProjectRejects", rows, headers, rowKeys);
            return result;
        }
        [HttpGet]
        public ActionResult GenerateProjectCommentsExcelReport()
        {
            string[] headers = new string[2] { "Project", "Comments"};
            string[] rowKeys = new string[2] { "p", "c"};
            IList rows;
            using (MatchingDB db = new MatchingDB())
            {
                rows = (from pr in db.Projects.OrderBy(p=>p.Name)
                        select new
                        {
                            p = pr.Name,
                            c = pr.Feedback
                        }).ToList();
            }
            var result = this.Excel("ProjectComments.xlsx", "Project Comments", rows, headers, rowKeys);
            return result;
        }

        [HttpGet]
        public ActionResult GenerateStudentCommentsExcelReport()
        {
            string[] headers = new string[2] { "Student", "Comments" };
            string[] rowKeys = new string[2] { "s", "c" };
            IList rows;
            using (MatchingDB db = new MatchingDB())
            {
                rows = (from st in db.Students.OrderBy(s => s.FirstName).ThenBy(s=>s.LastName)
                        select new
                        {
                            s = st.FirstName+" "+st.LastName,
                            c = st.OtherComments
                        }).ToList();
            }
            var result = this.Excel("StudentComments.xlsx", "Student Comments", rows, headers, rowKeys);
            return result;
        }
    }
}
