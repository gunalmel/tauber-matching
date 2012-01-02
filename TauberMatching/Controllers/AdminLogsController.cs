using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminLogsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /AdminLogs/
        [HttpGet]
        public ActionResult SystemLogs()
        {
            IList<SystemLogDto> systemLogs;
            using (MatchingDB db = new MatchingDB())
            {
                var systemLogSql=@"select * from log order by date desc";
                systemLogs = db.Database.SqlQuery<SystemLogDto>(systemLogSql,new object[0]).ToList();
            }
            return View(systemLogs);
        }
        [HttpGet]
        public ActionResult EmailLogs()
        {
            IList<EmailLogDto> emailLogs;
            using (MatchingDB db = new MatchingDB())
            {
                var emailLogSql = @"SELECT EmailLogs.Date, EmailLogs.Guid, EmailLogs.Status, EmailLogs.Subject, EmailLogs.Message, Projects.Name as Name, Projects.ContactFirst as FirstName, Projects.ContactLast as LastName, Projects.ContactEmail as Email
                                FROM EmailLogs INNER JOIN Projects ON EmailLogs.Project_Id = Projects.Id
                            UNION
                            SELECT  EmailLogs.Date, EmailLogs.Guid, EmailLogs.Status, EmailLogs.Subject, EmailLogs.Message, Students.UniqueName as Name, Students.FirstName as FirstName, Students.LastName as LastName, Students.Email as Email
                                FROM EmailLogs INNER JOIN Students ON EmailLogs.Student_Id = Students.Id
                            ORDER BY EmailLogs.Date DESC";
                emailLogs = db.Database.SqlQuery<EmailLogDto>(emailLogSql, new object[0]).ToList();
            }
            return View(emailLogs);
        }
        [HttpGet]
        public ActionResult EmailQueue()
        {            
            IList<EmailQueueMessage> emailQueueLogs;
            using (MatchingDB db = new MatchingDB())
            {
                emailQueueLogs = db.EmailQueueMessages.ToList();
            }
            return View(emailQueueLogs);
        }

        public ActionResult Clear(string id)
        {
            string redirect = "";
            string sql = "";
            switch (id)
            {
                case "systemLogs":
                    redirect = "SystemLogs";
                    sql = "DELETE FROM Log";
                    break;
                case "emailLogs":
                    redirect = "EmailLogs";
                    sql = "DELETE FROM EmailLogs";
                    break;
                case "emailQueue":
                    redirect = "EmailQueue";
                    sql = "DELETE FROM EmailQueueMessages";
                    break;
                default:
                    return RedirectToAction("SystemLogs");
            }
            using (MatchingDB db = new MatchingDB())
            {
                db.Database.ExecuteSqlCommand(sql, new object[0]);
            }
            return RedirectToAction(redirect);
        }
    }
}
