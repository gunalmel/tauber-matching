using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauberMatching.Models
{
    public class EmailLogDto
    {
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
