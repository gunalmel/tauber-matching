using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Models
{
    /// <summary>
    /// This class configures application behavior.
    /// </summary>
    public class AppConfiguration
    {
        private string _siteMasterFirstName;
        private string _siteMasterLastName;
        private string _siteMasterEmail;
        private string _siteMasterPhone;
        private int _minABusStudents;
        private int _minAEngStudents;
        private int _minAStudents;
        private int _maxRejectBusStudents;
        private int _maxRejectEngStudents;
        private int _maxRejectStudents;
        private bool _enforceContinuousProjectRanking;
        private bool _enforceContinuousStudentRanking;
        private int _projectRejectThreshold;
        private int _studentRejectThreshold; 

        /// <summary>
        /// First name of the person whose contact information is going to be sent with notification emails
        /// </summary>
        public string SiteMasterFirstName
        {
            get { return _siteMasterFirstName; }
            set { _siteMasterFirstName = value; }
        }
        /// <summary>
        /// Last name of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        public string SiteMasterLastName
        {
            get { return _siteMasterLastName; }
            set { _siteMasterLastName = value; }
        }
        /// <summary>
        /// Email of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        public string SiteMasterEmail
        {
            get { return _siteMasterEmail; }
            set { _siteMasterEmail = value; }
        }
        /// <summary>
        /// Phone# of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        public string SiteMasterPhone
        {
            get { return _siteMasterPhone; }
            set { _siteMasterPhone = value; }
        }
        /// <summary>
        /// Minimum number of business students who should be in A categeory
        /// </summary>
        public int MinABusStudents
        {
            get { return _minABusStudents; }
            set { _minABusStudents = value; }
        }
        /// <summary>
        /// Minimum number of engineering students who should be in A categeory
        /// </summary>
        public int MinAEngStudents
        {
            get { return _minAEngStudents; }
            set { _minAEngStudents = value; }
        }
        /// <summary>
        /// Minimum number of students who should be in A category
        /// </summary>
        public int MinAStudents
        {
            get { return _minAStudents; }
            set { _minAStudents = value; }
        }
        /// <summary>
        /// Maximum number of business students who can be rejected
        /// </summary>
        public int MaxRejectBusStudents
        {
            get { return _maxRejectBusStudents; }
            set { _maxRejectBusStudents = value; }
        }
        /// <summary>
        /// Maximum number of engineering students whou can be rejected
        /// </summary>
        public int MaxRejectEngStudents
        {
            get { return _maxRejectEngStudents; }
            set { _maxRejectEngStudents = value; }
        }
        /// <summary>
        /// Maximum number of students whou can be rejected
        /// </summary>
        public int MaxRejectStudents
        {
            get { return _maxRejectStudents; }
            set { _maxRejectStudents = value; }
        }
        /// <summary>
        /// Decides whether students should rank projects in a continuum. E.g.: If there are projects in 1st and 4th category then there should be projects in 2nd & 3rd category as well.
        /// </summary>
        public bool EnforceContinuousProjectRanking
        {
            get { return _enforceContinuousProjectRanking; }
            set { _enforceContinuousProjectRanking = value; }
        }
        /// <summary>
        /// Decides whether projects should rank students in a continuum. E.g.: If there are students in A and D category then there should be students in B & C category as well.
        /// </summary>
        public bool EnforceContinuousStudentRanking
        {
            get { return _enforceContinuousStudentRanking; }
            set { _enforceContinuousStudentRanking = value; }
        }
        /// <summary>
        /// If the number of projects are less than this number then students can not reject any projects
        /// </summary>
        public int ProjectRejectThreshold
        {
            get { return _projectRejectThreshold; }
            set { _projectRejectThreshold = value; }
        }
        /// <summary>
        /// If the number of students are less than this number then projects can not reject any students
        /// </summary>
        public int StudentRejectThreshold
        {
            get { return _studentRejectThreshold; }
            set { _studentRejectThreshold = value; }
        }
    }
}