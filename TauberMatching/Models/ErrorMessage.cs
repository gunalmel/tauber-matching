using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Models
{
    public class ErrorMessage
    {
        private const string INVALID_URL_MESSAGE = "Your access Url is invalid. Please contact Tauber Institute at The University Of Michigan for access.";
        private static readonly string TAUBER_EMAIL;
        private static readonly string TAUBER_PHONE;
        public static readonly string INVALID_URL_ERROR_MESSAGE;

        static ErrorMessage()
        {
            using(MatchingDB db = new MatchingDB() )
            {
                var scoreFor =ContactType.Project.ToString();
                TAUBER_EMAIL = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterEmail)).Value;
                TAUBER_PHONE = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterPhone)).Value;
                INVALID_URL_ERROR_MESSAGE = INVALID_URL_MESSAGE + "<br/><br/> Email: " + TAUBER_EMAIL + "<br/> PHONE:" + TAUBER_PHONE;
            }
        }
    }
}