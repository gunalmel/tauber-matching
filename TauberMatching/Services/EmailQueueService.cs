using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;

namespace TauberMatching.Services
{
    public class EmailQueueService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmailQueueService));
        /// <summary>
        /// Drops the message in the email queueing table for Quartz job to pick it up.
        /// </summary>
        /// <param name="to">Recipient</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Body of the email</param>
        public static void QueueMessage(EmailQueueMessage message)
        {
            using (var db = new MatchingDB())
            {
                db.EmailQueueMessages.Add(message);
                db.SaveChanges();
            }
        }

        private static IList<EmailQueueMessage> GetMessages(MatchingDB db)
        {
            IList<EmailQueueMessage> messages;
            messages = db.EmailQueueMessages.ToList();
            return messages;
        }

        private static void LogMessage(MatchingDB db, EmailQueueMessage qMessage,string status)
        {
            //TODO From the db ContactType field is missing. StudentId and ProjectId fields are irrelevant and not getting updated. Update Email log object as needed. #11
            EmailLog eLog = new EmailLog(qMessage,status);
            Project pr; Student st;
            switch (qMessage.ContactType)
            {
                case "Project":
                    pr = db.Projects.Where(p => p.Id == qMessage.ContactId).First();
                    pr.Emailed=(status==EmailStatus.Success.ToString());
                    if (pr.EmailLogs == null)
                        pr.EmailLogs = new List<EmailLog>();
                    pr.EmailLogs.Add(eLog);
                    break;
                case "Student":
                    st = db.Students.Where(s => s.Id == qMessage.ContactId).First();
                    st.Emailed = (status == EmailStatus.Success.ToString());
                    if (st.EmailLogs == null)
                        st.EmailLogs = new List<EmailLog>();
                    st.EmailLogs.Add(eLog);
                    break;
                default:
                    break;
            }
            db.SaveChanges();
        }
        public static void SendMailsInTheQueue()
        {
            int i = 0;
            using (var db = new MatchingDB())
            {
                //TODO Contacts emailed field is not being updated #12
                foreach (var m in GetMessages(db))
                {
                    EmailService emailService = new EmailService(m.To, m.Subject, m.Body);
                    String status = emailService.SendMessage();
                    LogMessage(db, m, status);
                    if (status == EmailStatus.Success.ToString())
                        db.EmailQueueMessages.Remove(m);
                    i++;
                }
                db.SaveChanges();
            }
            if(i>0)
                log.Info("The number of emails successfully sent: "+i.ToString()+" at "+DateTime.Now.ToString());
        }
    }
}