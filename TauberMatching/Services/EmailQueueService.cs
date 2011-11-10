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
            EmailLog eLog = new EmailLog(qMessage,status);
            db.EmailLogs.Add(eLog);
            db.SaveChanges();
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
                    if (status == "Success")
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