﻿using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace TauberMatching.Models
{
    public class Project
    {
        private string _name;
        private string _comments;
        private string _firstName;
        private string _lastName;
        private bool _emailed=false;
        private string _feedback;

        public Project() { }
        public Project(ProjectDTO pdto)
        {
            this.Name = pdto.Name;
            this.ContactFirst = pdto.ContactFirst;
            this.ContactLast = pdto.ContactLast;
            this.ContactEmail = pdto.ContactEmail;
            this.ContactPhone = pdto.ContactPhone;
        }

        [HiddenInput]
        public int Id { get; set; }
        [Required(ErrorMessage = "Mandatory Field: You must enter the name for the project!")]
        [DisplayName("Project Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Trim() != null)
                    _name = Regex.Replace(value.Trim(), @"\s+", " ");
                _name = value;
            }
        }
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
        [DisplayName("Contact First Name")]
        public string ContactFirst
        {
            get { return _firstName; }
            set
            {
                if (value != null && value.Trim() != null)
                    _firstName = Regex.Replace(value.Trim(), @"\s+", " ");
                _firstName = value;
            }
        }
        [DisplayName("Contact Last Name")]
        public string ContactLast
        {
            get { return _lastName; }
            set
            {
                if (value != null && value.Trim() != null)
                    _lastName = Regex.Replace(value.Trim(), @"\s+", " ");
                _lastName = value;
            }
        }
        [DisplayName("Contact Email")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Valid Email Address is required.")]
        public string ContactEmail { get; set; }
        [DisplayName("Contact Phone")]
        public string ContactPhone { get; set; }
        public virtual ICollection<Matching> Matchings { get; set; }
        public virtual ICollection<EmailLog> EmailLogs { get; set; }
        public virtual ICollection<UserError> UserErrorLogs { get; set; }
        public virtual ICollection<ProjectReject> ProjectRejects { get; set; }
        /// <summary>
        /// Feedback provided by the project owner as owner submitted student preferences
        /// </summary>
        public string Feedback
        {
            get { return _feedback; }
            set
            {
                if (value != null && value.Trim() != null)
                    _feedback = Regex.Replace(value.Trim(), @"\s+", " ");
                _feedback = value;
            }
        }
        /// <summary>
        /// The date when project owner submitted feedback and student choices
        /// </summary>
        public DateTime? ScoreDate { get; set; }
    }
    public class ProjectDTO
    {
        public string Name { get; set; }
        public string ContactFirst { get; set; }
        public string ContactLast { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }
    public class ProjectReject
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public String Reason { get; set; }
    }
}