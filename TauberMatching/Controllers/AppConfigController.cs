using System;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
    public class AppConfigController : Controller
    {
        private const string _updateMessage = "Configuration changes have been saved.";
        private const string _errorMessage = "There has been an error saving configuration cahnges. Copy the whole error message and send it to your application developer";
        // GET: /AppSetup/Edit
 
        public ActionResult Edit()
        {
            return View(ConfigurationService.GetConfigParameters());
        }

        //
        // POST: /AppSetup/Edit

        [HttpPost]
        public ActionResult Edit(AppConfiguration appConfig)
        {
            try
            {
                ConfigurationService.UpdateConfigParameters(appConfig);
                TempData["message"] = _updateMessage;
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
