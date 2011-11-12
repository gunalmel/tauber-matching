using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public String Status { get; set; }
        public String Subject { get; set; }
        public String Message { get; set; }
        public EmailLog() { }
        public EmailLog(EmailQueueMessage qMessage, String status)
        {
            Date = DateTime.Now;
            Guid = qMessage.Guid;
            Status = status;
            Subject = qMessage.Subject;
            Message = qMessage.Body;
        }
    }
}