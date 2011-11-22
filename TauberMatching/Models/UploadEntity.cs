using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using TauberMatching.Models;
using TauberMatching.Services;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    public class UploadEntity
    {
        private string _projectName;
        private string _contactFirst;
        private string _contactLast;

        private string _uniqueName;
        private string _studentFirst;
        private string _studentLast;

        public int Id { get; set; }
        [MaxLength(512, ErrorMessage = "Project name can be at most 512 characters long.")]
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (value != null && value.Trim() != null)
                    _projectName = Regex.Replace(value.Trim(), @"\s+", " ");
                _projectName = value;
            }
        }
        [MaxLength(128, ErrorMessage = "Contact first name can be at most 128 characters long.")]
        public string ContactFirst
        {
            get { return _contactFirst; }
            set
            {
                if (value != null && value.Trim() != null)
                    _contactFirst = Regex.Replace(value.Trim(), @"\s+", " ");
                _contactFirst = value;
            }
        }
        [MaxLength(128, ErrorMessage = "Contact last name can be at most 128 characters long.")]
        public string ContactLast
        {
            get { return _contactLast; }
            set
            {
                if (value != null && value.Trim() != null)
                    _contactLast = Regex.Replace(value.Trim(), @"\s+", " ");
                _contactLast = value;
            }
        }
        [MaxLength(256, ErrorMessage = "Contact email name can be at most 256 characters long.")]
        public String ContactEmail { get; set; }
        [MaxLength(16, ErrorMessage = "Phone number can be at most 16 characters long.")]
        public String ContactPhone { get; set; }
        [MaxLength(128, ErrorMessage = "Student unique name can be at most 128 characters long.")]
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
        [MaxLength(128, ErrorMessage = "Student first name can be at most 128 characters long.")]
        public string StudentFirst
        {
            get
            {
                return _studentFirst;
            }
            set
            {
                _studentFirst = Regex.Replace(value.Trim(), @"\s+", " ").InitCap();
            }
        }
        [MaxLength(128, ErrorMessage = "Student last name can be at most 128 characters long.")]
        public string StudentLast
        {
            get
            {
                return _studentLast;
            }
            set
            {
                _studentLast = Regex.Replace(value.Trim(), @"\s+", " ").InitCap();
            }
        }
        public String StudentDegree { get; set; }
        public int StudentId { get; set; }
        public int ProjectId { get; set; }
    }
}