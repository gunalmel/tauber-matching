using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;

namespace TauberMatching.Services
{
    public class ProjectService
    {
        private const string INVALID_URL_MESSAGE = "Your access Url is invalid. Please contact Tauber Institute at The University Of Michigan for access.";
        private static readonly string TAUBER_EMAIL;
        private static readonly string TAUBER_PHONE;
        public static readonly string INVALID_URL_ERROR_MESSAGE;
        public static readonly IList<ScoreDetail> ProjectScoreDetails;

        static ProjectService()
        {
            using(MatchingDB db = new MatchingDB() )
            {
                var scoreFor =ContactType.Project.ToString();
                ProjectScoreDetails = db.ScoreDetails.Where(sc => sc.ScoreFor == scoreFor).ToList();
                TAUBER_EMAIL = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterEmail)).Value;
                TAUBER_PHONE = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterPhone)).Value;
                INVALID_URL_ERROR_MESSAGE = INVALID_URL_MESSAGE + "<br/><br/> Email: " + TAUBER_EMAIL + "<br/> PHONE:" + TAUBER_PHONE;
            }
        }

        /// <summary>
        /// Using nullable guid fetches the project object eager loading all of its properties
        /// </summary>
        /// <param name="guid">Url unique identifier for the project</param>
        /// <returns>Project object</returns>
        public static Project GetProjectWithFullDetailsByGuid(Guid? guid)
        {
            if (guid == null)
                throw new ArgumentNullException("guid", "Project access url unique identifier is null no project can be fetched");
            Project project=null;
            using (MatchingDB db = new MatchingDB())
            {
                project = db.Projects.Include("Matchings").Include("Matchings.Student").FirstOrDefault(p => p.Guid == guid.Value);
            }
            return project;
        }

        /// <summary>
        /// Extracts all matching students from project and group them by the scores assigned to the students by the project contact in a dictionary whose key is the score value is the list of students who got that score.
        /// </summary>
        /// <param name="project">Deached project object which should have had its all properties eagerly loaded, otherwise an exception will be thrown.</param>
        /// <returns>A dictionary whose key is the score assigned to the list of students in the dictionary's value.</returns>
        public static IDictionary<ScoreDetail, IList<Student>> GetStudentsForProjectGroupedByScore(Project project)
        {
            if (project.Matchings == null || project.Matchings.Count == 0 || project.Matchings.Select(m => m.Student).Count() == 0)
                throw new ArgumentException("project", "There are no matching students for the project. Make sure all properties of your project was eagerly loaded before it was passed as parameter.");

            var dict = project.Matchings.GroupBy(m => ProjectScoreDetails.Where(sd => sd.Score == (m.ProjectScore ?? ProjectScore.NoScore.ToString())).FirstOrDefault()).ToDictionary(key => key.Key, value => value.Select(m => m.Student).ToList() as IList<Student>);
            foreach (ScoreDetail sd in ProjectScoreDetails)
            {
                if (!dict.Keys.Contains(sd))
                    dict.Add(sd, new List<Student>());
            }
            return dict;
        }

        private static ProjectScoreStudentCountMatrix GetStudentCountGroupedByDegreePerScore(IDictionary<ScoreDetail, IList<Student>> studentGroupedByScoreDict)
        {
            ProjectScoreStudentCountMatrix pssm = new ProjectScoreStudentCountMatrix();
            foreach (var entry in studentGroupedByScoreDict)
            {
                var degreeCountDict = entry.Value.GroupBy(s => s.Degree).ToDictionary(key => (StudentDegree)Enum.Parse(typeof(StudentDegree), key.Key), value => value.Count());
                foreach (var degreeCount in degreeCountDict)
                {
                    pssm[entry.Key.Score, degreeCount.Key] = degreeCount.Value;
                }
            }
            return pssm;
        }

        public static RankStudentsIndexModel BuildRankStudentsIndexModelForProject(Guid? guid)
        {
            RankStudentsIndexModel model;
            try
            {
                Project project = GetProjectWithFullDetailsByGuid(guid);
                IDictionary<ScoreDetail, IList<Student>> scoreGroupedStudents =  GetStudentsForProjectGroupedByScore(project);
                ProjectScoreStudentCountMatrix psscm = GetStudentCountGroupedByDegreePerScore(scoreGroupedStudents);
                model = new RankStudentsIndexModel(project.Name, scoreGroupedStudents, psscm);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == "project"||ex.ParamName =="guid")
                    model = new RankStudentsIndexModel(true, INVALID_URL_ERROR_MESSAGE);
                else
                    throw ex;
            }
            return model;
        }
    }
}