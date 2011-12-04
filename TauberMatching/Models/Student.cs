using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TauberMatching.Models;
using TauberMatching.Services;

namespace TauberMatching.Models
{
    public class Student
    {
        private string _uniqueName;
        private string _first;
        private string _last;
        private string _comments;
        private bool _emailed = false;

        public Student() { }
        public Student(StudentDTO sdto)
        {
            this.UniqueName = sdto.UniqueName;
            this.FirstName = sdto.StudentFirst;
            this.LastName = sdto.StudentLast;
            this.Email = sdto.UniqueName + "@umich.edu";
            this.Degree = sdto.Degree;
        }
        [HiddenInput]
        public int Id { get; set; }
        [Required(ErrorMessage = "Mandatory Field: You must enter the unique name for the student!")]
        [MaxLength(128, ErrorMessage = "Student unique name can be at most 128 characters long.")]
        [DisplayName("Unique Name")]
        public string UniqueName 
        { 
            get
            {
                return _uniqueName;
            }
            set
            {
                _uniqueName = Regex.Replace(value.Trim(), @"\s+", " ").ToLower();
            } 
        }
        [Required(ErrorMessage = "Mandatory Field: You must enter the first name of the student!")]
        [DisplayName("First Name")]
        [MaxLength(128, ErrorMessage = "Student first name can be at most 128 characters long.")]
        public string FirstName 
        {
            get
            {
                return _first;
            }
            set
            {
                _first = Regex.Replace(value.Trim(), @"\s+", " ").InitCap();
            }
        }
        [Required(ErrorMessage = "Mandatory Field: You must enter the last name of the student!")]
        [DisplayName("Last Name")]
        [MaxLength(128, ErrorMessage = "Student last name can be at most 128 characters long.")]
        public string LastName
        {
            get
            {
                return _last;
            }
            set
            {
                _last = Regex.Replace(value.Trim(), @"\s+", " ").InitCap();
            }
        }
        [Required(ErrorMessage = "Mandatory Field: You must choose the degree for the student!")]
        [DisplayName("Degree")]
        [MaxLength(16, ErrorMessage = "Degree can be at most 16 characters long. See Degree enum for possible values")]
        public String Degree { get; set; } // Bus: business, Eng:Engineering
        
        [Required(ErrorMessage = "Mandatory Field: You must enter the email for the student!")]
        [DisplayName("Email")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Valid Email Address is required.")]
        [MaxLength(256, ErrorMessage = "Student email name can be at most 256 characters long.")]
        public string Email { get; set; }
        
        [DisplayName("Emailed?")]
        public bool Emailed { get { return _emailed; } set { _emailed = value; } }
        /// <summary>
        /// Comments used on the administration interface
        /// </summary>
        [DisplayName("Comments")]
        public string Comments
        {
            get { return _comments; }
            set
            {
                if (value != null && value.Trim() != null)
                    _comments = Regex.Replace(value.Trim(), @"\s+", " ");
                _comments = value;
            }
        }
        [HiddenInput]
        public Guid Guid { get; set; }
        public virtual ICollection<Matching> Matchings { get; set; }
        public virtual ICollection<EmailLog> EmailLogs { get; set; }
        public virtual ICollection<StudentFeedback> StudentFeedbacks { get; set; }
        /// <summary>
        /// Comments provided by the student at the time when the student is submitting preferences.
        /// </summary>
        public string OtherComments { get; set; }
        /// <summary>
        /// The date when the student submitted the ranking form
        /// </summary>
        public DateTime? ScoreDate { get; set; }

        [NotMapped]
        public String FullName 
        {
            get
            {
                return (FirstName != null || LastName != null) ? (FirstName + " " + LastName) : "";
            }
        }
    }

    public class StudentFeedback
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Project Project { get; set; }
        [MaxLength(32, ErrorMessage = "Student feedback type can be at most 32 characters long.")]
        public string Type { get; set; } //Positive: positive feedback, Constructive: Constructive feedback
        public int FeedbackScore { get; set; }
    }

    public enum StudentDegree
    {
        Bus,Eng
    }

    public enum StudentFeedbackType { Positive, Constructive }

    public class StudentDTO
    {
        public String UniqueName { get; set; }
        public String StudentFirst { get; set; }
        public String StudentLast { get; set; }
        public String Degree { get; set; }
        public String Email { get; set; }
    }
}
