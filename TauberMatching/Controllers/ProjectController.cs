using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    [HandleError]
    public class ProjectController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectController));

        private String uniqueProjectNameErrorMsg="You have to pick a unique project name. There's already an existing project with the name you have specified.";
        MatchingDB db = new MatchingDB();

        public ActionResult Index()
        {
            var projects = from p in db.Projects select p;
            return View(projects.ToList());
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var project = from p in db.Projects where p.Id == id select p;
            return View(project.FirstOrDefault<Project>());
        }
        [HttpPost]
        public ActionResult Edit(Project p)
        {
            if (!ModelState.IsValid)
                return View(p);

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
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Project p = db.Projects.SingleOrDefault(pr => pr.Id == id);
            db.Projects.Remove(p);
            db.SaveChanges();
            TempData["message"] = "Project \"" + p.Name + "\" is deleted.";
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
                p.Guid = new Guid();
                db.Projects.Add(p);
                db.SaveChanges();
                TempData["message"] = "Project \"" + p.Name + "\" is added!";
                return RedirectToAction("Index");
            }
            return View(p);
        }
        private int GetProjectCountByName(String name)
        {
            return db.Projects.Where(p => p.Name.ToLower() == name.ToLower()).Count();
        }
        private bool ValidateProject(Project p)
        {
            return GetProjectCountByName(p.Name) == 0;
        }
    }
}
