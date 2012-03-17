using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;
using System.Text;

namespace TauberMatching.Services
{
    public class ProjectService
    {
        /// <summary>
        /// Using nullable guid fetches the project object eager loading all of its properties
        /// </summary>
        /// <param name="guid">Url unique identifier for the project</param>
        /// <returns>Project object</returns>
        public static Project GetProjectWithFullDetailsByGuid(Guid? guid)
        {
            if (guid == null)
                throw new ArgumentNullException("guid", "Project access url unique identifier is null or does not correspond to a valid Guid.");
            Project project = null;
            using (MatchingDB db = new MatchingDB())
            {
                project = db.Projects.Include("Matchings.Student").Include("ProjectRejects.Student").FirstOrDefault(p => p.Guid == guid.Value);
            }
            if(project==null)
                throw new ArgumentNullException("guid", "For the specified project access url unique identifier, no project can be fetched");
            return project;
        }

        /// <summary>
        /// Extracts all matching students from project and group them by the scores assigned to the students by the project contact in a dictionary whose key is the score and the value is the list of students who got that score.
        /// </summary>
        /// <param name="project">Detached project object which should have had its all properties eagerly loaded, otherwise an exception will be thrown.</param>
        /// <returns>A dictionary whose key is the score assigned to the list of students in the dictionary's value.</returns>
        public static IDictionary<ScoreDetail, IList<Student>> GetStudentsForProjectGroupedByScore(Project project)
        {
            if (project.Matchings == null || project.Matchings.Count == 0 || project.Matchings.Select(m => m.Student).Count() == 0)
                throw new ArgumentException("project", "There are no matching students for the project. Make sure all properties of your project was eagerly loaded before it was passed as parameter.");
           
            var unsortedDict = project.Matchings.GroupBy(m => UIParamsAndMessages.ProjectScoreDetails.Where(sd => sd.Score == m.ProjectScore).FirstOrDefault())
                                      .ToDictionary(key => key.Key, value => value.Select(m => m.Student).ToList() as IList<Student>);
            
            System.Collections.Generic.SortedDictionary<ScoreDetail,IList<Student>> dict = new System.Collections.Generic.SortedDictionary<ScoreDetail,IList<Student>>(unsortedDict);

            foreach (ScoreDetail sd in UIParamsAndMessages.ProjectScoreDetails)
            {
                if (!dict.Keys.Contains(sd))
                    dict.Add(sd, new List<Student>());
            }
            return dict;
        }

        public static IDictionary<StudentDegree, int> GetStudentsCountForProjectGroupedByDegree(Project project)
        {
            if (project.Matchings == null || project.Matchings.Count == 0 || project.Matchings.Select(m => m.Student).Count() == 0)
                throw new ArgumentException("project", "There are no matching students for the project. Make sure all properties of your project was eagerly loaded before it was passed as parameter.");

            var dict = project.Matchings.GroupBy(m => m.Student.Degree).ToDictionary(key => (StudentDegree)Enum.Parse(typeof(StudentDegree), key.Key), value => value.Select(m => m.Student).ToList().Count);
            foreach (StudentDegree sd in Enum.GetValues(typeof(StudentDegree)))
            {
                if (!dict.Keys.Contains(sd))
                    dict.Add(sd, 0);
            }
            return dict;
        }
        public static RankStudentsIndexModel GetRankStudentsIndexModelForProject(Guid? guid)
        {
            RankStudentsIndexModel model;
            try
            {
                Project project = GetProjectWithFullDetailsByGuid(guid);
				IDictionary<ScoreDetail, IList<Student>> scoreGroupedStudents = GetStudentsForProjectGroupedByScore(project);
                IDictionary<StudentDegree, int> degreeGroupedStudentCount = GetStudentsCountForProjectGroupedByDegree(project);
                IDictionary<Student, string> projectRejects = project.ProjectRejects.ToDictionary(key =>key.Student, value => value.Reason);
                model = new RankStudentsIndexModel(project.Id, project.Guid.ToString(), project.Name, scoreGroupedStudents, projectRejects, project.Feedback, degreeGroupedStudentCount);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == "project" || ex.ParamName == "guid")
                    model = new RankStudentsIndexModel(true, UIParamsAndMessages.INVALID_URL_ERROR_MESSAGE);
                else
                    throw ex;
            }
            return model;
        }

        /// <summary>
        /// Returns the list of projects which id not interview with the student as specified by the student identifier.
        /// </summary>
        /// <param name="studentId">Student identifier in the db</param>
        /// <returns>List of projects which did not interview the selected student</returns>
        public static IList<Project> GetProjectsNotMatchingStudent(int studentId)
        {
            IList<Project> projects = null;
            using (MatchingDB db = new MatchingDB())
            {
                projects = db.Projects.Where(p => !p.Matchings.Select(m=>m.Student.Id).Contains(studentId)).ToList();
            }
            return projects;
        }
        /// <summary>
        /// Returns the model that will be returned to the details view that is returned by projects controller's details action method
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns><see cref="ProjectDetailsModel"/></returns>
        public static ProjectDetailsModel GetProjectDetailsModelForProject(int projectId)
        {
            Project project = null;
            using (MatchingDB db = new MatchingDB())
            {
                project=db.Projects.Where(p => p.Id == projectId).FirstOrDefault();
            }
            return new ProjectDetailsModel(project.Id,project.Name,StudentService.GetStudentDtoNotMatchingProject(projectId), StudentService.GetStudentDtoForProject(projectId));
        }
        /// <summary>
        /// Deletes all matching objects for a given project from the database.
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        public static void DeleteMatchingsForProject(int projectId)
        {
            using (MatchingDB db = new MatchingDB())
            {
                Project project = db.Projects.Include("Matchings.Student").Include("ProjectRejects").Where(p => p.Id == projectId).FirstOrDefault();
                var existingMatchings = project.Matchings.ToList();
                // Find the children collection of students that used be matching the project but will not be matching following the deletion.
                var studentsRemovedFromProject = existingMatchings.Select(m => m.Student.Id).ToArray();
                var projectRejectsToRemove =project.ProjectRejects.Where(pr=>studentsRemovedFromProject.Contains(pr.Student.Id)).ToList();
                
                project.Matchings.Clear();
                foreach (Matching m in existingMatchings)
                    db.Matchings.Remove(m);

                #region Clear the collection for the students deleted off the db.
                foreach (ProjectReject pr in projectRejectsToRemove)
                    db.ProjectRejects.Remove(pr);
                #endregion
                db.SaveChanges();
             // Not needed because only non-matching students should be providing Positive|Constructive Feedback StudentService.DeleteStudentFeedbacksReferencingProjectForStudents(projectId, studentsRemovedFromProject);
            }
        }

        /// <summary>
        /// Replaces all the matching students within the list of matching objects of a project with the students specified by student id array provided as argument.
        /// </summary>
        /// <param name="studentIdsToAdd">List of student identifiers that will constitute the new matching list of the given project</param>
        public static void ReplaceMatchingsForProjectWith(int projectId, int[] studentIdsToAdd)
        {
            using (MatchingDB db = new MatchingDB())
            {
                Project project = db.Projects.Include("Matchings.Student").Include("ProjectRejects").Where(p => p.Id == projectId).FirstOrDefault();
                if (project.Matchings == null)
                    project.Matchings = new List<Matching>();
                IList<Matching> existingProjectMatchings = project.Matchings.ToList();
                IList<Matching> matchingsToRemove = existingProjectMatchings.Where(m => !studentIdsToAdd.Contains(m.Student.Id)).ToList();
                int[] studentIdsToRemove = matchingsToRemove.Select(m => m.Student.Id).ToArray();
                int[] newStudentIds = studentIdsToAdd.Where(sId => !existingProjectMatchings.Select(m => m.Student.Id).Contains(sId)).ToArray();
                var projectRejectsToRemove = project.ProjectRejects.Where(pr => studentIdsToRemove.Contains(pr.Student.Id)).ToList();
                #region Remove the students that is not on the UI but within the existing matchings of the projects and then add the students that did not appear within the list of existing matchings of the project but appeared on the UI
                //Remove students that do not appear in the list of students to add
                foreach (var m in matchingsToRemove)
                {
                    db.Matchings.Remove(m);
                }    
                // Add students that did not exist before
                foreach (var sId in newStudentIds)
                {
                    Student st = db.Students.Where(s => s.Id == sId).FirstOrDefault();
                    Matching m = new Matching() { Project = project, Student = st, ProjectScore = ProjectScore.NoScore.ToString(), StudentScore = StudentScore.NoScore.ToString() };
                    project.Matchings.Add(m);
                }
                #endregion
                #region Clear the collection for the students replaced off the db.
                foreach (ProjectReject pr in projectRejectsToRemove)
                    db.ProjectRejects.Remove(pr);
                #endregion

                //ICollection<Matching> matchings = new List<Matching>();
                //var existingMatchingsToBeReplaced = db.Matchings.Where(m=>m.Project.Id==projectId).ToList();
                // Find the children collection of students that used be matching the project but will not be matching following the replacement.
                //var studentsRemovedFromProject = db.Matchings.Include("Student.StudentFeedbacks").Where(m=>m.Project.Id==projectId&&!studentIdsToAdd.Contains(m.Student.Id)).Select(m=>m.Student.Id).ToArray();
                //var projectRejectsToRemove = project.ProjectRejects.Where(pr => studentsRemovedFromProject.Contains(pr.Student.Id)).ToList();

                //foreach (int studentId in studentIdsToAdd)
                //{
                //    Student st = db.Students.Where(s => s.Id == studentId).FirstOrDefault();
                //    Matching m = new Matching() { Project = project, Student = st,ProjectScore=ProjectScore.NoScore.ToString(), StudentScore=StudentScore.NoScore.ToString() };
                //    matchings.Add(m);
                //}
                //#region Clear the collection for the students replaced off the db.
                //foreach (ProjectReject pr in projectRejectsToRemove)
                //    db.ProjectRejects.Remove(pr);
                //foreach (Matching m in existingMatchingsToBeReplaced)
                //    db.Matchings.Remove(m);
                //#endregion
                //project.Matchings = matchings;
                db.SaveChanges();
                // Not needed because only non-matching students should be providing Positive|Constructive Feedback StudentService.DeleteStudentFeedbacksReferencingProjectForStudents(projectId, studentsRemovedFromProject);
            }
        }

        public static IList<ProjectDto> GetProjecDtoNotMatchingStudent(int studentId)
        {
            IList<ProjectDto> projects = null;
            using (MatchingDB db = new MatchingDB())
            {
                projects = db.Projects.Where(s => !s.Matchings.Select(m => m.Student.Id).Contains(studentId)).OrderBy(p => p.Name).Select(p => new ProjectDto() { Id = p.Id, Name = p.Name }).ToList();
            }
            return projects;
        }

        public static IList<ProjectDto> GetProjectDtoForStudent(int studentId)
        {
            IList<ProjectDto> projects = null;
            using (MatchingDB db = new MatchingDB())
            {
                projects = db.Projects.Where(s => s.Matchings.Select(m => m.Student.Id).Contains(studentId)).OrderBy(p => p.Name).Select(p => new ProjectDto() { Id = p.Id, Name = p.Name }).ToList();
            }
            return projects;
        }

        public static Project DeleteProject(int projectId)
        {
            Project p = null;
            DeleteMatchingsForProject(projectId);
            StudentService.DeleteStudentFeedbacksReferencingProject(projectId);
            using (MatchingDB db = new MatchingDB())
            {
                p = db.Projects.SingleOrDefault(pr => pr.Id == projectId);
                db.Projects.Remove(p);
                db.SaveChanges();
            }
            return p;
        }

        public static void DeleteProjectRejectsReferencingStudent(int studentId)
        {
            using(MatchingDB db = new MatchingDB())
            {
                var projectRejectsToDelete = db.ProjectRejects.Where(pr => pr.Student.Id == studentId).ToList();
                foreach (ProjectReject pr in projectRejectsToDelete)
                    db.ProjectRejects.Remove(pr);
                db.SaveChanges();
            }
        }

        public static void DeleteProjectRejectsReferencingStudentForProjects(int studentId, params int[] projectIds)
        {
            using (MatchingDB db = new MatchingDB())
            {
                var projectRejectsToDelete = db.Projects.Where(p => projectIds.Contains(p.Id)).Select(p => p.ProjectRejects.Where(pr=>pr.Student.Id==studentId).FirstOrDefault());

                foreach (ProjectReject pr in projectRejectsToDelete)
                {
                    if (pr != null) // The projection above would insert null into projectRejectsToDelete list for the studentId when the project has not rejected that student.
                        db.ProjectRejects.Remove(pr);
                }
                db.SaveChanges();
            }
        }

    }
}