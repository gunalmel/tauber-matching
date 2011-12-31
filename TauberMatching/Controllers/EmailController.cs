using System.Collections.Generic;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;
using System.Linq;
using System;

namespace TauberMatching.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmailController : Controller
    {
        [HttpPost]
        public void SendAccessUrl(IList<Contact> contacts)
        {
            foreach (var c in contacts)
            {
                var emailType = c.ContactType == ContactType.Project ? EmailType.ProjectAccess : EmailType.StudentAccess;
                var eqm = new EmailQueueMessage(c, emailType);
                EmailQueueService.QueueMessage(eqm);
            }
        }
    }
}
