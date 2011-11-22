using System.Collections.Generic;
using System.Web.Mvc;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Controllers
{
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
