using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;
using System.Collections;

namespace TauberMatching.Services
{
    public class StudentService
    {
        /// <summary>
        /// Using nullable guid fetches the student object eager loading all of its properties
        /// </summary>
        /// <param name="guid">Url unique identifier for the student</param>
        /// <returns>Student object</returns>
        public static Student GetStudentWithFullDetailsByGuid(Guid? guid)
        {
            if (guid == null)
                throw new ArgumentNullException("guid", "Student access url unique identifier is null or does not correspond to a valid Guid.");
            Student student = null;
            using (MatchingDB db = new MatchingDB())
            {
                student = db.Students.Include("Matchings.Project").Include("StudentFeedbacks.Project").FirstOrDefault(s => s.Guid == guid.Value);
            }
            if (student == null)
                throw new ArgumentNullException("guid", "For the specified student access url unique identifier, no student can be fetched");
            return student;
        }
        /// <summary>
        /// Extracts all matching projects from student and group them by the scores assigned to the projects by the student in a dictionary whose key is the score and the value is the list of rpojects who got that score.
        /// </summary>
        /// <param name="project">Detached student object which should have had its all properties eagerly loaded, otherwise an exception will be thrown.</param>
        /// <returns>A dictionary whose key is the score assigned to the list of students in the dictionary's value.</returns>
        public static IDictionary<ScoreDetail, IList<Project>> GetProjectsForStudentGroupedByScore(Student student)
        {
            if (student.Matchings == null || student.Matchings.Count == 0 || student.Matchings.Select(m => m.Student).Count() == 0)
                throw new ArgumentException("There are no matching projects for the student. Make sure all properties of your student was eagerly loaded before it was passed as parameter.", "student");

            var unsortedDict = student.Matchings.GroupBy(m => UIParamsAndMessages.StudentScoreDetails.Where(sd => sd.Score == m.StudentScore).FirstOrDefault()).ToDictionary(key => key.Key, value => value.Select(m => m.Project).ToList() as IList<Project>);
            
            System.Collections.Generic.SortedDictionary<ScoreDetail, IList<Project>> dict = new System.Collections.Generic.SortedDictionary<ScoreDetail, IList<Project>>(unsortedDict);
            
            foreach (ScoreDetail sd in UIParamsAndMessages.StudentScoreDetails)
            {
                if (!dict.Keys.Contains(sd))
                    dict.Add(sd, new List<Project>());
            }
            return dict;
        }

        public static RankProjectsIndexModel GetRankProjectsIndexModelForStudent(Guid? guid)
        {
            RankProjectsIndexModel model;
            try
            {
                Student student = GetStudentWithFullDetailsByGuid(guid);
                IDictionary<ScoreDetail, IList<Project>> scoreGroupedProjects = GetProjectsForStudentGroupedByScore(student);
                IList<Project> projectsNotInterviewed = ProjectService.GetProjectsNotMatchingStudent(student.Id);
                var positiveTypeName = StudentFeedbackType.Positive.ToString();
                var constructiveTypeName = StudentFeedbackType.Constructive.ToString();
                IDictionary<int, int> positiveFeedbacks = student.StudentFeedbacks.Where(sf => sf.Type == positiveTypeName).ToDictionary(key => key.Project.Id, value => value.FeedbackScore);
                IDictionary<int, int> constructiveFeedbacks = student.StudentFeedbacks.Where(sf => sf.Type == constructiveTypeName).ToList().ToDictionary(key => key.Project.Id, value => value.FeedbackScore);
                model = new RankProjectsIndexModel(student.Id, student.Guid.ToString(), student.FullName, scoreGroupedProjects, student.OtherComments, projectsNotInterviewed, positiveFeedbacks, constructiveFeedbacks);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == "project" || ex.ParamName == "guid")
                    model = new RankProjectsIndexModel(true, UIParamsAndMessages.INVALID_URL_ERROR_MESSAGE);
                else
                    throw ex;
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName=="student")
                    model = new RankProjectsIndexModel(true, "There are no matchings for this student in the db yet. Interview data has not been entered.");
                else
                    throw ex;
            }
            return model;
        }

        public static IList<StudentDto> GetStudentDtoForProject(int projectId)
        {
            IList<StudentDto> students=null;
            using(MatchingDB db = new MatchingDB())
            {
                students = db.Students.Where(s => s.Matchings.Select(m => m.Project.Id).Contains(projectId)).OrderBy(s => s.FirstName).ThenBy(s => s.LastName).Select(s => new StudentDto() { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName, Degree = s.Degree }).ToList();
            }
            return students;
        }
        public static IList<StudentDto> GetStudentDtoNotMatchingProject(int projectId)
        {
            IList<StudentDto> students = null;
            using (MatchingDB db = new MatchingDB())
            {
                students = db.Students.Where(s => !s.Matchings.Select(m => m.Project.Id).Contains(projectId)).OrderBy(s => s.FirstName).ThenBy(s=>s.LastName).Select(s => new StudentDto() { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName, Degree = s.Degree }).ToList();
            }
            return students;
        }
        /// <summary>
        /// Deletes all matching objects for a given student from the database.
        /// </summary>
        /// <param name="studentId">Student identifier</param>
        public static void DeleteMatchingsForStudent(int studentId)
        {
            using (MatchingDB db = new MatchingDB())
            {
                Student student = db.Students.Include("Matchings.Project").Include("StudentFeedbacks").Where(s => s.Id == studentId).FirstOrDefault();
                var existingMatchings = student.Matchings.ToList();
                var studentFeedbacksToBeDeleted = student.StudentFeedbacks.ToList();

                student.Matchings.Clear();
                foreach (Matching m in existingMatchings)
                    db.Matchings.Remove(m);

                #region Delete the student feedbacks of the student for the projects that appeared in the matchings deleted.
                foreach (StudentFeedback sf in studentFeedbacksToBeDeleted)
                    db.StudentFeedbacks.Remove(sf);
                #endregion
                db.SaveChanges();
                ProjectService.DeleteProjectRejectsReferencingStudent(studentId);
            }
        }
        /// <summary>
        /// Updates matchings for a student with the user selections on Student Details screen
        /// </summary>
        /// <param name="projectIdsToAdd">List of project identifiers that will constitute the new matching list of the given student</param>
        public static void ReplaceMatchingsForStudentWith(int studentId, int[] projectIdsToAdd)
        {
            using (MatchingDB db = new MatchingDB())
            {
                Student student = db.Students.Include("Matchings.Project").Include("StudentFeedbacks.Project").Where(s => s.Id == studentId).FirstOrDefault();
                if (student.Matchings == null)
                    student.Matchings = new List<Matching>();
                IList<Matching> existingStudentMatchings = student.Matchings.ToList();
                IList<Matching> matchingsToRemove = existingStudentMatchings.Where(m => !projectIdsToAdd.Contains(m.Project.Id)).ToList();
                int[] projectIdsToRemove = matchingsToRemove.Select(m => m.Project.Id).ToArray();
                int[] newProjectIds = projectIdsToAdd.Where(pId => !existingStudentMatchings.Select(m => m.Project.Id).Contains(pId)).ToArray();
                int[] projectsRemovedFromStudent = matchingsToRemove.Where(m => m.Student.Id == studentId && !projectIdsToAdd.Contains(m.Project.Id)).Select(m => m.Project.Id).ToArray();
                IList<StudentFeedback> studentFeedbacksToBeDeleted = student.StudentFeedbacks.Where(sf => newProjectIds.Contains(sf.Project.Id)).ToList();

                //Remove projects that do not appear in the list of projects to add
                foreach (var m in matchingsToRemove)
                    db.Matchings.Remove(m);
                // Add projects that did not exist before
                foreach (var pId in newProjectIds)
                {
                    Project project = db.Projects.Where(p => p.Id == pId).FirstOrDefault();
                    Matching m = new Matching() { Project = project, Student = student, ProjectScore = ProjectScore.NoScore.ToString(), StudentScore = StudentScore.NoScore.ToString() };
                    db.Matchings.Add(m);
                }
                foreach (var sf in studentFeedbacksToBeDeleted)
                    db.StudentFeedbacks.Remove(sf);

                db.SaveChanges();
                ProjectService.DeleteProjectRejectsReferencingStudentForProjects(studentId, projectsRemovedFromStudent);
            }
        }

        /// <summary>
        /// Returns the model that will be returned to the details view that is returned by projects controller's details action method
        /// </summary>
        /// <param name="studentId">Project Identifier</param>
        /// <returns><see cref="StudentDetailsModel"/></returns>
        public static StudentDetailsModel GetStudentDetailsModelForStudent(int studentId)
        {
            Student student = null;
            using (MatchingDB db = new MatchingDB())
            {
                student = db.Students.Where(s => s.Id == studentId).FirstOrDefault();
            }
            return new StudentDetailsModel(student.Id, student.FullName, ProjectService.GetProjecDtoNotMatchingStudent(studentId), ProjectService.GetProjectDtoForStudent(studentId));
        }

        public static Student DeleteStudent(int studentId)
        {
            Student s = null;
            DeleteMatchingsForStudent(studentId);
            using (MatchingDB db = new MatchingDB())
            {
                s = db.Students.Include("EmailLogs").SingleOrDefault(st => st.Id == studentId);

                #region Remove EmailLogs for the project
                IList<EmailLog> emailLogsToBeDeleted = s.EmailLogs.ToList();
                foreach (EmailLog log in emailLogsToBeDeleted)
                    db.EmailLogs.Remove(log);
                db.SaveChanges();
                #endregion

                db.Students.Remove(s);
                db.SaveChanges();
            }
            return s;
        }

        public static void DeleteStudentFeedbacksReferencingProjectForStudents(int projectId, params int[] studentsRemovedFromProject)
        {
            using (MatchingDB db = new MatchingDB())
            {
               var studentFeedbacksToDelete = db.StudentFeedbacks.Where(sf => sf.Project.Id == projectId && studentsRemovedFromProject.Contains(sf.Student.Id)).ToList();
               foreach (StudentFeedback sf in studentFeedbacksToDelete)
               {
                   if (sf != null)// The projection above would insert null into projectRejectsToDelete list for the studentId when the project has not rejected that student.
                       db.StudentFeedbacks.Remove(sf);
               }
               db.SaveChanges();
            }
        }

        public static void DeleteStudentFeedbacksReferencingProject(int projectId)
        {
            using (MatchingDB db = new MatchingDB())
            {
                var studentFeedbacksToDelete = db.StudentFeedbacks.Where(sf => sf.Project.Id == projectId).ToList();
                foreach (StudentFeedback sf in studentFeedbacksToDelete)
                    db.StudentFeedbacks.Remove(sf);
                db.SaveChanges();
            }
        }
    }
}