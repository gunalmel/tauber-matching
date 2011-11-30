using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    public class StudentController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectController));
        private String uniqueNameErrorMsg = "There's already an existing student with the unique name you specified.";

        public ActionResult Index()
        {
            MatchingDB db = new MatchingDB();
            var students = db.Students.ToList();
            db.Dispose();
            return View(students);
        }

        //
        // GET: /Student/Details/5
        //TODO #19 Display project-student matchings and let admin user add new student matching
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(StudentService.GetStudentDetailsModelForStudent(id));
        }
        [HttpPost]
        public ActionResult Details(int id, FormCollection form)
        {
            int[] projectIdsToAdd = null;
            string formInput;
            try
            {
                if ((formInput = form["selectInterviewed"]) != null)
                {
                    projectIdsToAdd = formInput.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                    StudentService.ReplaceMatchingsForStudentWith(id, projectIdsToAdd);
                }
                else
                {
                    StudentService.DeleteMatchingsForStudent(id);
                }
                TempData["message"] = "Your changes are saved.";
            }
            catch (Exception ex)
            {
                log.Error("Error during updating the project list for a student with id: " + id.ToString(), ex.InnerException != null ? ex.InnerException : ex);
                TempData["error"] = true;
                TempData["message"] = "An error occured while updating the student. Contact your system administrator with the following info: Student id:" + id.ToString() + " Timestamp of the incident: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            }
            return RedirectToAction("Details", new { id = id });
        }

        //
        // GET: /Student/Create
        [HttpGet]
        public ActionResult Create()
        {
            setViewDataForListOfDegrees();
            return View();
        } 

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(Student st)
        {
            if (!ValidateStudent(st))
                this.ModelState.AddModelError("UniqueName", uniqueNameErrorMsg);
            if (ModelState.IsValid)
            {
                MatchingDB db = new MatchingDB();
                st.Guid = new Guid();
                db.Students.Add(st);
                db.SaveChanges();
                TempData["message"] = "Student \"" + st.FirstName+" "+st.LastName + "\" is added!";
                db.Dispose();
                return RedirectToAction("Index");
            }
            setViewDataForListOfDegrees();
            return View(st);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            MatchingDB db = new MatchingDB();
            var student = (from s in db.Students where s.Id == id select s).FirstOrDefault<Student>();
            setViewDataForListOfDegrees();
            db.Dispose();
            return View(student);
        }
        [HttpPost]
        public ActionResult Edit(Student st)
        {
            if (!ModelState.IsValid)
                return View(st);
            MatchingDB db = new MatchingDB();
            Student student = db.Students.FirstOrDefault(s => s.Id == st.Id);
            if (student.UniqueName.ToLower() != st.UniqueName.ToLower() && !ValidateStudent(st))
            {
                setViewDataForListOfDegrees();
                this.ModelState.AddModelError("UniqueName", uniqueNameErrorMsg);
                return View(st);
            }

            student.UniqueName = st.UniqueName;
            student.LastName = st.LastName;
            student.FirstName = st.FirstName;
            student.Email = st.Email;
            student.Degree = st.Degree;
            student.Comments = st.Comments;
            db.SaveChanges();
            TempData["message"] = "Student \"" + student.FirstName+" "+student.LastName + "\" is updated.";
            db.Dispose();
            return RedirectToAction("Index");
        }

        //
        // GET: /Student/Delete/5
 
        public ActionResult Delete(int id)
        {
            MatchingDB db = new MatchingDB();
            Student s = db.Students.SingleOrDefault(st => st.Id == id);
            db.Students.Remove(s);
            db.SaveChanges();
            TempData["message"] = "Student \"" + s.FirstName+" "+s.LastName + "\" is deleted.";
            db.Dispose();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Sets view data with the key degrees to the list of available degrees option for a student so that relevant screen element can bind to it.
        /// </summary>
        private void setViewDataForListOfDegrees()
        {
            ViewData["degrees"] = from String d in Enum.GetNames(typeof(StudentDegree)) select d;
        }
        private int GetStudentCountByUniqueName(String uname)
        {
            MatchingDB db = new MatchingDB();
            int count=db.Students.Where(s => s.UniqueName == uname.ToLower()).Count();
            db.Dispose();
            return count;
        }
        private bool ValidateStudent(Student s)
        {
            return GetStudentCountByUniqueName(s.UniqueName) == 0;
        }
    }
}
