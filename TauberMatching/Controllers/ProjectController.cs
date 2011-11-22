using System;
using System.Linq;
using System.Web.Mvc;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    [HandleError]
    public class ProjectController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectController));
        private String uniqueProjectNameErrorMsg="You have to pick a unique project name. There's already an existing project with the name you have specified.";

        public ActionResult Index()
        {
            MatchingDB db = new MatchingDB();
            var projects = db.Projects.ToList();
            Project p = projects.FirstOrDefault();
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
        //TODO #19 Display project-student matchings and let admin user add new project matching
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Delete(int id)
        {
            MatchingDB db = new MatchingDB();
            Project p = db.Projects.SingleOrDefault(pr => pr.Id == id);
            db.Projects.Remove(p);
            db.SaveChanges();
            TempData["message"] = "Project \"" + p.Name + "\" is deleted.";
            db.Dispose();
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
                p.Guid = new Guid();
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
            int count =  db.Projects.Where(p => p.Name.ToLower() == name.ToLower()).Count();
            db.Dispose();
            return count;
        }
        private bool ValidateProject(Project p)
        {
            return GetProjectCountByName(p.Name) == 0;
        }
    }
}
