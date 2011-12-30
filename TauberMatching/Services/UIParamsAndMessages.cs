using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;
using System.Text;

namespace TauberMatching.Services
{
    /// <summary>
    /// A data structure class that stores messages, config parameters needed by UIs for the selected contact type.
    /// </summary>
    public class UIParamsAndMessages
    {
        /// <summary>
        /// Tauber admin full name for all users to contact in case of a problem.
        /// </summary>
        public static readonly string TAUBER_ADMIN_NAME;
        public static readonly string TAUBER_EMAIL;
        public static readonly string TAUBER_PHONE;
        /// <summary>
        /// Error message to be displayed to the users when the unique ccess url identifier does not match any user info in the db.
        /// </summary>
        public static readonly string INVALID_URL_ERROR_MESSAGE;
        public static readonly string STUDENT_DEGREE_JAVASCRIPT_ARRAY;
        /// <summary>
        /// A string in js that sets an array storing scores that students can use to rank projects
        /// </summary>
        public static readonly string STUDENT_SCORES_JAVASCRIPT_ARRAY;
        /// <summary>
        /// A string in js that sets an array storing scores that projects can use to rank students
        /// </summary>
        public static readonly string PROJECT_SCORES_JAVASCRIPT_ARRAY;

        public static readonly string PROJECT_CONFIG_PARAM_JS_ARRAY;

        public static readonly string STUDENT_CONFIG_PARAM_JS_ARRAY;
        /// <summary>
        /// The js statements string that will be injected in the RankStudents Index View
        /// </summary>
        public static readonly string RANK_STUDENTS_INDEX_HEAD;
        /// <summary>
        /// The js statements string that will be injected in the RankProjects Index View
        /// </summary>
        public static readonly string RANK_PROJECTS_INDEX_HEAD;
        /// <summary>
        /// Scores that can be used by projects to rank students
        /// </summary>
        public static readonly IList<ScoreDetail> ProjectScoreDetails;
        /// <summary>
        /// Scores that can be used by the students to rank theprojects
        /// </summary>
        public static readonly IList<ScoreDetail> StudentScoreDetails;

        static UIParamsAndMessages()
        {
            using(MatchingDB db = new MatchingDB())
            {
                TAUBER_ADMIN_NAME=db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterFirstName).FirstOrDefault().Value+" "+db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterLastName).FirstOrDefault().Value;
                TAUBER_EMAIL=db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterEmail).FirstOrDefault().Value;
                TAUBER_PHONE=db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterPhone).FirstOrDefault().Value;
                INVALID_URL_ERROR_MESSAGE = String.Format(ConfigurationService.GetEmailConfigParameters().InvalidAccessUrlMessage/*System.Configuration.ConfigurationManager.AppSettings["ProjectAccessUrlSubject"]*/, TAUBER_EMAIL, TAUBER_PHONE);

                ProjectScoreDetails = ScoreService.GetScoreDetailsFor(ContactType.Project);
                StudentScoreDetails = ScoreService.GetScoreDetailsFor(ContactType.Student);

                STUDENT_DEGREE_JAVASCRIPT_ARRAY = StudentDegreeService.GetStudentDegreeJavaScritArray();

                PROJECT_SCORES_JAVASCRIPT_ARRAY = ScoreService.GetScoreJavascriptArrayFromScoreDetails(ProjectScoreDetails);
                STUDENT_SCORES_JAVASCRIPT_ARRAY = ScoreService.GetScoreJavascriptArrayFromScoreDetails(StudentScoreDetails);

                PROJECT_CONFIG_PARAM_JS_ARRAY = ConfigurationService.GetBusinessRulesConfigParametersAsJSVariableStatementFor(ContactType.Project);
                STUDENT_CONFIG_PARAM_JS_ARRAY = ConfigurationService.GetBusinessRulesConfigParametersAsJSVariableStatementFor(ContactType.Student);

                RANK_STUDENTS_INDEX_HEAD = new StringBuilder().Append(STUDENT_DEGREE_JAVASCRIPT_ARRAY).Append(PROJECT_SCORES_JAVASCRIPT_ARRAY).Append(PROJECT_CONFIG_PARAM_JS_ARRAY).ToString();
                RANK_PROJECTS_INDEX_HEAD = new StringBuilder().Append(STUDENT_SCORES_JAVASCRIPT_ARRAY).Append(STUDENT_CONFIG_PARAM_JS_ARRAY).ToString();
            }
        }       
    }
}