﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    public class StudentController : Controller
    {
        private String uniqueNameErrorMsg = "There's already an existing student with the unique name you have specified.";
        MatchingDB db = new MatchingDB();

        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        //
        // GET: /Student/Details/5

        public ActionResult Details(int id)
        {
            return View();
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
                st.Guid = new Guid();
                db.Students.Add(st);
                db.SaveChanges();
                TempData["message"] = "Student \"" + st.FirstName+" "+st.LastName + "\" is added!";
                return RedirectToAction("Index");
            }
            setViewDataForListOfDegrees();
            return View(st);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var student = from s in db.Students where s.Id == id select s;
            setViewDataForListOfDegrees();
            return View(student.FirstOrDefault<Student>());
        }
        [HttpPost]
        public ActionResult Edit(Student st)
        {
            if (!ModelState.IsValid)
                return View(st);

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
            return RedirectToAction("Index");
        }

        //
        // GET: /Student/Delete/5
 
        public ActionResult Delete(int id)
        {
            Student s = db.Students.SingleOrDefault(st => st.Id == id);
            db.Students.Remove(s);
            db.SaveChanges();
            TempData["message"] = "Student \"" + s.FirstName+" "+s.LastName + "\" is deleted.";
            return RedirectToAction("Index");
        }

        //
        // POST: /Student/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        /// <summary>
        /// Sets view data with the key degrees to the list of available degrees option for a student so that relevant screen element can bind to it.
        /// </summary>
        private void setViewDataForListOfDegrees()
        {
            ViewData["degrees"] = from String d in Enum.GetNames(typeof(Degree)) select d;
        }
        private int GetStudentCountByUniqueName(String uname)
        {
            return db.Students.Where(s => s.UniqueName == uname.ToLower()).Count();
        }
        private bool ValidateStudent(Student s)
        {
            return GetStudentCountByUniqueName(s.UniqueName) == 0;
        }
    }
}
