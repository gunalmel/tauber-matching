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
        private static IList<ScoreDetail> StudentScoreDetails=ScoreService.GetScoreDetailsFor(ContactType.Student);

        public static Student GetStudentWithFullDetailsByGuid(Guid? guid)
        {
            if (guid == null)
                throw new ArgumentNullException("guid", "Student access url unique identifier is null no project can be fetched");
            Student student = null;
            using (MatchingDB db = new MatchingDB())
            {
                student = db.Students.Include("Matchings").Include("Matchings.Project").FirstOrDefault(s => s.Guid == guid.Value);
            }
            return student;
        }

        public static IDictionary<ScoreDetail, IList<Project>> GetProjectsForStudentGroupedByScore(Student student)
        {
            if (student.Matchings == null || student.Matchings.Count == 0 || student.Matchings.Select(m => m.Student).Count() == 0)
                throw new ArgumentException("student", "There are no matching projects for the student. Make sure all properties of your student was eagerly loaded before it was passed as parameter.");

            var dict = student.Matchings.GroupBy(m => StudentScoreDetails.Where(sd => sd.Score == (m.StudentScore ?? StudentScore.NoScore.ToString())).FirstOrDefault()).ToDictionary(key => key.Key, value => value.Select(m => m.Project).ToList() as IList<Project>);
            foreach (ScoreDetail sd in StudentScoreDetails)
            {
                if (!dict.Keys.Contains(sd))
                    dict.Add(sd, new List<Project>());
            }
            return dict;
        }

        public static RankProjectsIndexModel BuildRankProjectsIndexModelForProject(Guid? guid)
        {
            RankProjectsIndexModel model;
            try
            {
                Student student = GetStudentWithFullDetailsByGuid(guid);
                IDictionary<ScoreDetail, IList<Project>> scoreGroupedProjects = GetProjectsForStudentGroupedByScore(student);
                model = new RankProjectsIndexModel(student.FullName, scoreGroupedProjects);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == "project" || ex.ParamName == "guid")
                    model = new RankProjectsIndexModel(true, ErrorMessage.INVALID_URL_ERROR_MESSAGE);
                else
                    throw ex;
            }
            return model;
        }

    }
}