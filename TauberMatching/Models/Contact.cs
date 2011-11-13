using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TauberMatching.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public ContactType ContactType { get; set; }
        public string Email { get; set; }
        public Guid Guid{get;set;}
        public Contact(Project p)
        {
            this.Id = p.Id;
            this.FirstName = p.ContactFirst;
            this.LastName = p.ContactLast;
            this.Email = p.ContactEmail;
            this.Guid = p.Guid;
            this.ContactType = ContactType.Project;
        }
        public Contact(Student s)
        {
            this.Id = s.Id;
            this.FirstName = s.FirstName;
            this.LastName = s.LastName;
            this.Email = s.Email;
            this.Guid = s.Guid;
            this.ContactType = ContactType.Student;
        }
    }
    public enum ContactType { Student, Project }
}