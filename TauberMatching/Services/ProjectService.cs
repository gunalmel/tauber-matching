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
               projects = db.Matchings.Where(m => m.Student.Id != studentId).Select(m => m.Project).Distinct().ToList();
            }
            return projects;
        }
    }
}