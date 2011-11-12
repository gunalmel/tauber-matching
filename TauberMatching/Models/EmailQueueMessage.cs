using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    /// <summary>
    /// Whenever an email is sent by the user or the system the email message is dropped in the database to form a queue that will be fed into Quartz scheduled jobs
    /// </summary>
    public class EmailQueueMessage
    {
        //TODO Add Message Templating Issue#7
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string ContactType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Guid { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailQueueMessage() { }
        public EmailQueueMessage(int contactId,string contactType,string firstName,string lastName,Guid guid, string to, string subject, string body)
        {
            ContactId = contactId;
            ContactType = contactType;
            FirstName = firstName;
            LastName = lastName;
            Guid = guid;
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}