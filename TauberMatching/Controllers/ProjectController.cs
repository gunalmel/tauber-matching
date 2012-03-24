using System;
using System.Linq;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    [HandleError]
    [Authorize(Roles = "Administrator")]
    public class ProjectController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectController));
        private String uniqueProjectNameErrorMsg = "You have to pick a unique project name. There's already an existing project with the name you have specified.";

        public ActionResult Index()
        {
            MatchingDB db = new MatchingDB();
            var projects = db.Projects.Include("Matchings").OrderBy(pr => pr.Name).ToList();
            db.Dispose();
            return View(projects);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            MatchingDB db = new MatchingDB();
            var project = (from p in db.Projects where p.Id == id select p).FirstOrDefault<Project>();
            db.Dispose();
            return View(project);
        }
        [HttpPost]
        public ActionResult Edit(Project p)
        {
            if (!ModelState.IsValid)
                return View(p);
            MatchingDB db = new MatchingDB();
            Project pr = db.Projects.FirstOrDefault(project => project.Id == p.Id);

            if (pr.Name.ToLower() != p.Name.ToLower() && !ValidateProject(p))
            {
                this.ModelState.AddModelError("Name", uniqueProjectNameErrorMsg);
                return View(p);
            }

            pr.Name = p.Name;
            pr.Comments = p.Comments;
            pr.ContactFirst = p.ContactFirst;
            pr.ContactLast = p.ContactLast;
            pr.ContactEmail = p.ContactEmail;
            pr.ContactPhone = p.ContactPhone;
            db.SaveChanges();
            TempData["message"] = "Project \"" + pr.Name + "\" is updated.";
            db.Dispose();
            return RedirectToAction("Index");
        }
        //
        // GET: /Student/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(ProjectService.GetProjectDetailsModelForProject(id));
        }
        [HttpPost]
        public ActionResult Details(int id, FormCollection form)
        {
            int[] studentIdsToAdd = null;
            string formInput;
            try
            {
                if ((formInput = form["selectInterviewed"]) != null)
                {
                    studentIdsToAdd = formInput.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                    ProjectService.ReplaceMatchingsForProjectWith(id, studentIdsToAdd);
                }
                else
                {
                    ProjectService.DeleteMatchingsForProject(id);
                }
                TempData["message"] = "Your changes are saved.";
            }
            catch (Exception ex)
            {
                log.Error("Error during updating the student list for a project with id: " + id.ToString(), ex.InnerException != null ? ex.InnerException : ex);
                TempData["error"] = true;
                TempData["message"] = "An error occured while updating the project. Contact your system administrator with the following info: Project id:" + id.ToString() + " Timestamp of the incident: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            }
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Delete(int id)
        {
            Project p;
            string message = "";
            try
            {
                p = ProjectService.DeleteProject(id);
                message = "Project " + p.Name + " is deleted.";
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error while deleting project with id: " + id.ToString(), ex.InnerException ?? ex);
                message = "Unexpected error while deleting the project with id: " + id.ToString() + " Contact support with the error messge, project id, date and time of the error.";
            }
            TempData["message"] = message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Project p)
        {
            if (!ValidateProject(p))
                this.ModelState.AddModelError("Name", uniqueProjectNameErrorMsg);

            if (ModelState.IsValid)
            {
                MatchingDB db = new MatchingDB();
                p.Guid = Guid.NewGuid();
                db.Projects.Add(p);
                db.SaveChanges();
                TempData["message"] = "Project \"" + p.Name + "\" is added!";
                db.Dispose();
                return RedirectToAction("Index");
            }
            return View(p);
        }
        private int GetProjectCountByName(String name)
        {
            MatchingDB db = new MatchingDB();
            int count = db.Projects.Where(p => p.Name.ToLower() == name.ToLower()).Count();
            db.Dispose();
            return count;
        }
        private bool ValidateProject(Project p)
        {
            return GetProjectCountByName(p.Name) == 0;
        }
    }
}
