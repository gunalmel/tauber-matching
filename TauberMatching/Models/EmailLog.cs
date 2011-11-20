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
        [MaxLength(16, ErrorMessage = "Status can be at 16 characters long. Success|Failed See EmailStatus enum in EmailService")]
        public String Status { get; set; }
        [MaxLength(512, ErrorMessage = "Email subject that will be logged can be at most 512 characters long.")]
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