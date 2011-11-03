using System;
using System.Collections.Generic;

namespace TauberMatching.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public char ContactType { get; set; }
        public int ContactId { get; set; }
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public String Status { get; set; }
        public String Message { get; set; }
    }
}