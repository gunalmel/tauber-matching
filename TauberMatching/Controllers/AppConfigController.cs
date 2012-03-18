using System;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AppConfigController : Controller
    {
        private const string _updateMessage = "Configuration changes have been saved.";
        private const string _errorMessage = "There has been an error saving configuration changes. Copy the whole error message and send it to your application developer";
        // GET: /AppSetup/Edit
        
        public ActionResult ClearDatabase()
        {
            var sqlEmailLogs = "DELETE FROM EmailLogs";
            var sqlEmailQueueMessages = "DELETE FROM EmailQueueMessages";
            var sqlLog = "DELETE FROM Log";
            var sqlMatchings = "DELETE FROM Matchings";
            var sqlProjectRejects = "DELETE FROM ProjectRejects";
            var sqlProjects = "DELETE FROM Projects";
            var sqlStudentFeedbacks = "DELETE FROM StudentFeedbacks";
            var sqlStudents = "DELETE FROM Students";
            var sqlUploadEntities = "DELETE FROM UploadEntities";
            string connString;
            using (MatchingDB db = new MatchingDB())
            {
                db.Database.ExecuteSqlCommand(sqlEmailLogs, new object[0]);
                db.Database.ExecuteSqlCommand(sqlEmailQueueMessages, new object[0]);
                db.Database.ExecuteSqlCommand(sqlLog, new object[0]);
                db.Database.ExecuteSqlCommand(sqlMatchings, new object[0]);
                db.Database.ExecuteSqlCommand(sqlProjectRejects, new object[0]);
                db.Database.ExecuteSqlCommand(sqlStudentFeedbacks, new object[0]);
                db.Database.ExecuteSqlCommand(sqlProjects, new object[0]);
                db.Database.ExecuteSqlCommand(sqlStudents, new object[0]);
                db.Database.ExecuteSqlCommand(sqlUploadEntities, new object[0]);
                connString = db.Database.Connection.ConnectionString;
            }
            System.Web.HttpRuntime.UnloadAppDomain();
            System.Data.SqlServerCe.SqlCeEngine engine = new System.Data.SqlServerCe.SqlCeEngine(connString);
            engine.Shrink();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Edit()
        {
            return View(ConfigurationService.GetConfigParameters());
        }

        //
        // POST: /AppSetup/Edit

        [HttpPost]
        public ActionResult Edit(AppConfiguration appConfig)
        {
            try
            {
                ConfigurationService.UpdateConfigParameters(appConfig);
                TempData["message"] = _updateMessage;
                System.Web.HttpRuntime.UnloadAppDomain();
                return View();
            }
            catch(Exception ex)
            {
                TempData["message"] = _errorMessage+ex.Message;
                if (ex.InnerException != null)
                    TempData["message"] = _errorMessage + ex.Message + " Inner Exception: " + ex.InnerException.Message;
                return View();
            }
        }
    }

    [Authorize(Roles = "Administrator")]
    public class EmailConfigController : Controller
    {
        private const string _updateMessage = "Configuration changes have been saved.";
        private const string _errorMessage = "There has been an error saving configuration changes. Copy the whole error message and send it to your application developer";
        
        public ActionResult Edit()
        {
            return View(ConfigurationService.GetEmailConfigParameters());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EmailConfiguration emailConfig)
        {
            try
            {
                ConfigurationService.UpdateEmailConfigParameters(emailConfig);
                TempData["message"] = _updateMessage;
                System.Web.HttpRuntime.UnloadAppDomain();
                return View(ConfigurationService.GetEmailConfigParameters());
            }
            catch (Exception ex)
            {
                TempData["message"] = _errorMessage + ex.Message;
                if (ex.InnerException != null)
                    TempData["message"] = _errorMessage + ex.Message + " Inner Exception: " + ex.InnerException.Message;
                return View();
            }
        }
    }
}
