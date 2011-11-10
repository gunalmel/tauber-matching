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
        /// <summary>
        /// Retrieves all application settings in AppConfiguration object
        /// </summary>
        /// <returns>AppConfiguration object that encapsulates all named application configuration parameters.</returns>
        public static AppConfiguration GetConfigParameters()
        {
            AppConfiguration appConfig;
            using (MatchingDB db = new MatchingDB())
                appConfig = new AppConfiguration(db.ConfigParameters);
            return appConfig;
        }
        /// <summary>
        /// Updates the config parameters in the database matching the properties of the method argument
        /// </summary>
        /// <param name="config">AppConfiguration object encapsulating application configuration parameters to be set in the db.</param>
        public static void UpdateConfigParameters(AppConfiguration config)
        {
            using (MatchingDB db = new MatchingDB())
            {
                IEnumerable<ConfigParameter> parameters = config.GetConfigParameters();
                foreach (var param in parameters)
                {
                    var pm = db.ConfigParameters.FirstOrDefault(p => p.Id == param.Id);
                    if (pm == null)
                        db.ConfigParameters.Add(param);
                    else
                        pm.Value = param.Value;
                }
                db.SaveChanges();
            }
        }
    }
}