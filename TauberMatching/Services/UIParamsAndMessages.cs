using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;

namespace TauberMatching.Services
{
    /// <summary>
    /// A data structure class that stores messages, config parameters needed by UIs for the selected contact type.
    /// </summary>
    public class UIParamsAndMessages
    {
        public static readonly string TAUBER_ADMIN_NAME;
        public static readonly string TAUBER_EMAIL;
        public static readonly string TAUBER_PHONE;
        public static readonly string INVALID_URL_ERROR_MESSAGE;
        public static readonly IList<ScoreDetail> ProjectScoreDetails;
        public static readonly IList<ScoreDetail> StudentScoreDetails;

        static UIParamsAndMessages()
        {
            using(MatchingDB db = new MatchingDB())
            {
                TAUBER_ADMIN_NAME=db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterFirstName).FirstOrDefault().Value+" "+db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterLastName).FirstOrDefault().Value;
                TAUBER_EMAIL=db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterEmail).FirstOrDefault().Value;
                TAUBER_PHONE=db.ConfigParameters.Where(cp=>cp.Id==(int)ConfigEnum.SiteMasterPhone).FirstOrDefault().Value;
                INVALID_URL_ERROR_MESSAGE = String.Format(System.Configuration.ConfigurationManager.AppSettings["ProjectAccessUrlSubject"], TAUBER_EMAIL, TAUBER_PHONE);

                ProjectScoreDetails = ScoreService.GetScoreDetailsFor(ContactType.Project);
                StudentScoreDetails = ScoreService.GetScoreDetailsFor(ContactType.Student);
            }
        }
    }
}