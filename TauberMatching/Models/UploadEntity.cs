using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using TauberMatching.Services;

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
        public String ContactEmail { get; set; }
        public String ContactPhone { get; set; }
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