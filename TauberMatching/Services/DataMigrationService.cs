using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;
using System.Collections;
using System.Configuration;
using System.Data.SqlServerCe;
using TauberMatching.Services;

namespace TauberMatching.Services
{
    public class DataMigrationService
    {
        MatchingDB db = new MatchingDB();
        /// <summary>
        /// Moves the data from UploadEntities table whic stores Project and Student matchings to project, student and matchings table.
        /// </summary>
        public void MigrateFromUploadedEntities()
        {
            CreateProjects(ExtractProjects());
            CreateStudents(ExtractStudents());
            CreateMatchings(ExtractMatchings());
        }
        /// <summary>
        /// Extracts the list of projects from UploadEntities table which holds the data imported from student-project matching sheet.
        /// </summary>
        /// <returns>List of unique project records from the student-project list</returns>
        public IList<ProjectDTO> ExtractProjects()
        {
            var projectDtos= db.UploadEntities.Select(ue => new ProjectDTO() { Name = ue.ProjectName, ContactFirst = ue.ContactFirst, ContactLast = ue.ContactLast, ContactEmail = ue.ContactEmail, ContactPhone = ue.ContactPhone }).Distinct().ToList<ProjectDTO>();
            return projectDtos;
        }
        /// <summary>
        /// Extracts the list of students from UploadEntities table which holds the data imported from student-project matching sheet.
        /// </summary>
        /// <returns>List of distinct student records from the student-project matching list</returns>
        public IList<StudentDTO> ExtractStudents()
        {
            var studentDtos = db.UploadEntities.Select(ue => new StudentDTO() { UniqueName = ue.UniqueName, StudentFirst = ue.StudentFirst, StudentLast = ue.StudentLast, Degree = ue.StudentDegree, Email = ue.UniqueName.ToLower() + "@umich.edu" }).Distinct().ToList<StudentDTO>();
            return studentDtos;
        }
        public void CreateProjects(IList<ProjectDTO> pdtos)
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            DeleteEntity(EntityType.Matchings);
            DeleteEntity(EntityType.Projects);
            foreach (var pdto in pdtos)
            {
                Project p = new Project(pdto);
                db.Projects.Add(p);
                db.SaveChanges();
                Func<UploadEntity, bool> setId = (ue) => { ue.ProjectId = p.Id; return true; };
                db.UploadEntities.Where(ue => ue.ProjectName == pdto.Name).All(setId);
                db.SaveChanges();
            }
            db.Configuration.ValidateOnSaveEnabled = true;
        }
        public void DeleteEntity(EntityType e)
        {
            using (SqlCeConnection conn = new SqlCeConnection(InitializeConnectionString()))
            {
                conn.Open();
                string deleteTable = "delete from " + e.ToString();
                SqlCeCommand cmdDelete = new SqlCeCommand(deleteTable, conn);
                cmdDelete.ExecuteNonQuery();
            }
        }
        public void CreateStudents(IList<StudentDTO> sdtos)
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            DeleteEntity(EntityType.Matchings);
            DeleteEntity(EntityType.Students);
            foreach (var sdto in sdtos)
            {
                Student s = new Student(sdto);
                db.Students.Add(s);
                db.SaveChanges();
                Func<UploadEntity, bool> setId = (ue) => { ue.StudentId = s.Id; return true; };
                db.UploadEntities.Where(ue => ue.UniqueName == sdto.UniqueName).All(setId);
                db.SaveChanges();
            }
            db.Configuration.ValidateOnSaveEnabled = true;
        }
        /// <summary>
        /// Extract the ids to represent student project matchings from uploaded data.
        /// </summary>
        /// <returns>List of student id/project id matchings to be inserted into production tables from uploadentities table</returns>
        public IList<MatchingDTO> ExtractMatchings()
        {
            return db.UploadEntities.Select(ue => new MatchingDTO() { ProjectId = ue.ProjectId, StudentId = ue.StudentId }).ToList();
        }

        public void CreateMatchings(IList<MatchingDTO> mdtos)
        {
            DeleteEntity(EntityType.Matchings);
            foreach (var mdto in mdtos)
            {
                var project = db.Projects.Where(p => p.Id == mdto.ProjectId).First();
                var student = db.Students.Where(s => s.Id == mdto.StudentId).First();
                var matching = new Matching() { Project = project, Student = student };
                db.Matchings.Add(matching);
                db.SaveChanges();
            }
        }
        private String InitializeConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MatchingDB"].ConnectionString;
        }
    }
    public enum EntityType { Projects, Students, UploadEntities, Matchings }
}