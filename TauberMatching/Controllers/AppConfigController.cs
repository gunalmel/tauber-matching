using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Models;

namespace TauberMatching.Controllers
{
    public class AppConfigController : Controller
    {
        private const string _updateMessage = "Configuration changes have been saved.";
        private const string _errorMessage = "There has been an error saving configuration cahnges. Copy the whole error message and send it to your application developer";
        // GET: /AppSetup/Edit
 
        public ActionResult Edit()
        {
            MatchingDB db = new MatchingDB();
            AppConfiguration appConfig = new AppConfiguration(db.ConfigParameters);
            return View(appConfig);
        }

        //
        // POST: /AppSetup/Edit

        [HttpPost]
        public ActionResult Edit(AppConfiguration appConfig)
        {
            try
            {
                MatchingDB db = new MatchingDB();
                IEnumerable<ConfigParameter> parameters = appConfig.GetConfigParameters();
                foreach (var param in parameters)
                {
                    var pm = db.ConfigParameters.FirstOrDefault(p => p.Id == param.Id);
                    if (pm == null)
                        db.ConfigParameters.Add(param);
                    else
                        pm.Value = param.Value;
                }
                TempData["message"] = _updateMessage;
                db.SaveChanges();
                return View();
            }
            catch(Exception ex)
            {
                TempData["message"] = _errorMessage+ex.Message;
                if (ex.InnerException != null)
                    TempData["message"] = _errorMessage + ex.Message + " Inner Exception: " + ex.InnerException.Message;
                return View();
            }
        }
    }
}
