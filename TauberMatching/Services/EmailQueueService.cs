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
        /// Drops the message in the email queueing table for Quartz job to pick it up. This is the main method to send templated -emails to project contacts and students.
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
            EmailLog eLog = new EmailLog(qMessage,status);
            Project pr; Student st;
            try
            {
                switch (qMessage.ContactType)
                {
                    case "Project":
                        pr = db.Projects.Where(p => p.Id == qMessage.ContactId).First();
                        pr.Emailed = (status == EmailStatus.Success.ToString());
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
            catch (Exception ex)
            {
                log.Error("An unexpected exception occured while creating a log record for the notification e-mail sent from the queue. The id for the " + qMessage.ContactType + " is: " + qMessage.ContactId + ". You can disregard this message if the " + qMessage.ContactType + " is deleted.", ex.InnerException ?? ex);
            }
        }
        public static void SendMailsInTheQueue()
        {
            int i = 0;
            using (var db = new MatchingDB())
            {
                foreach (var m in GetMessages(db))
                {
                    EmailService emailService = new EmailService(m.To, m.Subject, m.Body);
                    String status = emailService.SendMessage();
                    LogMessage(db, m, status);
                    if (status == EmailStatus.Success.ToString())
                        db.EmailQueueMessages.Remove(m);
                    i++;
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    log.Info("An error has happened while trying to persist the log info after e-mail has been sent and emaillog has been generated but before queue message has been deleted. Exception is:",ex);
                }
            }
            if(i>0)
                log.Info("The number of emails successfully sent: "+i.ToString()+" at "+DateTime.Now.ToString());
        }
    }
}