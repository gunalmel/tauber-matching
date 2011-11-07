using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;

namespace TauberMatching.Services
{
    /// <summary>
    /// Service class to manipulate application configuration that alters business logic or specifies application constants such as admin contact.
    /// </summary>
    public class ConfigurationService
    {
        MatchingDB db = new MatchingDB();
        /// <summary>
        /// Retrieves all application settings in AppConfiguration object
        /// </summary>
        /// <returns>AppConfiguration object that encapsulates all named application configuration parameters.</returns>
        public AppConfiguration GetConfigParameters()
        {
            var appConfig = new AppConfiguration(db.ConfigParameters);
            return appConfig;
        }

        public void UpdateConfigParameters(AppConfiguration config)
        {
            
        }
    }
}