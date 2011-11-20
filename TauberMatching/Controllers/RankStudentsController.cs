using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    public class RankStudentsController : Controller
    {
        public enum IndexModelAttributes { ScoreDetails, ProjectName, GroupedStudents }
        private const string INVALID_URL_MESSAGE = "Your access Url is invalid. Please contact University of Michigan Tauber Institute for access.";
        private static readonly string TAUBER_EMAIL;
        private static readonly string TAUBER_PHONE;
        private static readonly IList<ScoreDetail> _projectScoreDetails;

        static RankStudentsController()
        {
            using(MatchingDB db = new MatchingDB() )
            {
                var scoreFor =ContactType.Project.ToString();
                _projectScoreDetails = db.ScoreDetails.Where(sc => sc.ScoreFor == scoreFor).ToList();
                TAUBER_EMAIL = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterEmail)).Value;
                TAUBER_PHONE = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterPhone)).Value;
            }
        }

        public ActionResult Index(Guid? id)
        {
            return View(GetModel(id));
        }

        private IDictionary<IndexModelAttributes,Object> GetModel(Guid? id)
        {
            var modelEntries = GetModelEntriesForIndexPage(id);
            String projectName = modelEntries.Key;
            IDictionary<string, List<Student>> groupedStudents = modelEntries.Value;

            IDictionary<IndexModelAttributes, Object> model = new Dictionary<IndexModelAttributes, object>();
            model.Add(IndexModelAttributes.ScoreDetails, _projectScoreDetails);
            model.Add(IndexModelAttributes.ProjectName, projectName);
            model.Add(IndexModelAttributes.GroupedStudents, groupedStudents);
            return model;
        }

        private KeyValuePair<String, IDictionary<String, List<Student>>> GetModelEntriesForIndexPage(Guid? guid)
        {
            KeyValuePair<String, IDictionary<String, List<Student>>> keyValue;
            if (guid == null)
                keyValue = GetInvalidAccessUrlModelForIndexPage();
            else
            {
                Project project;
                using (MatchingDB db = new MatchingDB())
                {
                    project = db.Projects.Include("Matchings").Include("Matchings.Student").FirstOrDefault(p => p.Guid == guid.Value);
                }
                keyValue = project == null ? GetInvalidAccessUrlModelForIndexPage() : GetModelEntriesForIndexPage(project);
            }
            return keyValue;
        }

        /// <summary>
        /// Returns the model for Index view that will be used when Access Url does not correspond to a project.
        /// </summary>
        /// <returns>KeyValuePair model object for Index page. Key is project name, value is the dictionary of students whose key is the score assigned by the project</returns>
        private KeyValuePair<String, IDictionary<String,List<Student>>> GetInvalidAccessUrlModelForIndexPage()
        {
            string errorMessage = INVALID_URL_MESSAGE + "<br/><br/> Email: " + TAUBER_EMAIL + "<br/> PHONE:" + TAUBER_PHONE;
            return new KeyValuePair<String, IDictionary<String,List<Student>>>(errorMessage, null);

        }

        private KeyValuePair<String, IDictionary<String,List<Student>>> GetModelEntriesForIndexPage(Project project)
        {
            var dict = project.Matchings.GroupBy(m => (m.ProjectScore??ProjectScore.NoScore.ToString())).ToDictionary(key => key.Key, value => value.Select(m => m.Student).ToList());
            return new KeyValuePair<string,IDictionary<string,List<Student>>>(project.Name,dict);
        }
    }
}
