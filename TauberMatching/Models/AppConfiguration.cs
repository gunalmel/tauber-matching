﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    /// <summary>
    /// Encapsulates all config parameters for the applicaion behavior each of which would be mapped to the db hthorugh ConfigParameter class and ConfigEnum enum.
    /// This class configures application behavior. 
    /// Builds a bridge between UI and database backed string attribute/value pairs that represents application config parameters
    /// Properties of this class should match ConfigEnum  entries
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
        private int _minBBusStudents;
        private int _minBEngStudents;
        private int _minBStudents;
        private int _maxRejectedBusStudents;
        private int _maxRejectedEngStudents;
        private int _maxRejectedStudents;
        private int _minFirstProjects;
        private int _maxRejectedProjects;
        private bool _enforceContinuousProjectRanking;
        private bool _enforceContinuousStudentRanking;
        private int _rejectedProjectThreshold;
        private int _rejectedStudentThreshold;

        private Type thisType = typeof(AppConfiguration);

        public AppConfiguration() { }
        public AppConfiguration(IEnumerable<ConfigParameter> parameters)
        {
            foreach (var param in parameters)
            {
                if (param.Value != null && param.Value.Length > 0)
                {
                    PropertyInfo prop = thisType.GetProperty(param.Name);
                    Type t = prop.PropertyType;
                    Object o = Convert.ChangeType(param.Value, t);
                    prop.SetValue(this, o, null);
                }
            }
        }
        private ConfigParameter GetConfigParameter(ConfigEnum name)
        {
            int paramId = (int)name;
            String propName = Enum.GetName(typeof(ConfigEnum), name);
            var valueObject=thisType.GetProperty(propName).GetValue(this, null);
            String value = String.Empty;
            if (valueObject != null)
                value = valueObject.ToString();
            return new ConfigParameter(name, value);
        }
        /// <summary>
        /// Gets or sets instance properties by ConfigParameter object
        /// </summary>
        /// <param name="configParam">Configuration parameter as specified by the ConfigEnum</param>
        /// <returns>Instance of Configuration parameter object to manage persistence</returns>
        public ConfigParameter this[ConfigEnum configParam]
        {
            get { return GetConfigParameter(configParam); }
            set
            {
                if (value.Value != null && value.Value.Length > 0)
                {
                    PropertyInfo prop = thisType.GetProperty(Enum.GetName(typeof(ConfigEnum), configParam));
                    Type t = prop.PropertyType;
                    Object o = Convert.ChangeType(value.Value, t);
                    prop.SetValue(this, o, null);
                }
            }
        }
        /// <summary>
        /// Iterating through not null properties builds the collection of config parameters by mapping properties of this instance to named config parameters
        /// </summary>
        /// <returns>Enumeration of ConfigParameter objects that represents persistable application configuration settings.</returns>
        public IEnumerable<ConfigParameter> GetConfigParameters()
        {
            IList<ConfigParameter> configParameters = new List<ConfigParameter>();
            foreach(ConfigEnum e in Enum.GetValues(typeof(ConfigEnum)))
            {
                configParameters.Add(this[e]);
            }
            return configParameters;
        }
        /// <summary>
        /// First name of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        [DisplayName("Site Admin First Name:")]
        public string SiteMasterFirstName
        {
            get { return _siteMasterFirstName; }
            set 
            { 
                if(value!=null)
                    _siteMasterFirstName = value.Trim(); 
            }
        }
        /// <summary>
        /// Last name of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        [DisplayName("Site Admin Last Name:")]
        public string SiteMasterLastName
        {
            get { return _siteMasterLastName; }
            set 
            {
                if (value != null)
                    _siteMasterLastName = value.Trim(); 
            }
        }
        /// <summary>
        /// Email of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        [DisplayName("Site Admin Email:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the email for the site admin for users to reply back when there is a problem !")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Valid Email Address is required. Besides requiring the correct e-mail format, trailing/preceding spaces in e-mail addresses are not allowed.)")]  
        public string SiteMasterEmail
        {
            get { return _siteMasterEmail; }
            set { _siteMasterEmail = value; }
        }
        /// <summary>
        /// Phone# of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        [DisplayName("Site Admin Phone:")]
        public string SiteMasterPhone
        {
            get { return _siteMasterPhone; }
            set 
            {
                if (value != null)
                    _siteMasterPhone = value.Trim(); 
            }
        }
        /// <summary>
        /// Minimum number of business students who should be in A categeory
        /// </summary>
        [DisplayName("Min # of Business Students to be ranked as Ideal (A):")]
        public int MinABusStudents
        {
            get { return _minABusStudents; }
            set { _minABusStudents = value; }
        }
        /// <summary>
        /// Minimum number of engineering students who should be in A categeory
        /// </summary>
        [DisplayName("Min # of Engineering Students to be ranked as Ideal (A):")]
        public int MinAEngStudents
        {
            get { return _minAEngStudents; }
            set { _minAEngStudents = value; }
        }
        /// <summary>
        /// Minimum number of students who should be in A category
        /// </summary>
        [DisplayName("Min total # of Students to be ranked as Ideal (A):")]
        public int MinAStudents
        {
            get { return _minAStudents; }
            set { _minAStudents = value; }
        }

        /// <summary>
        /// Minimum number of business students who should be in B categeory
        /// </summary>
        [DisplayName("Min # of Business Students to be ranked as Desired (B):")]
        public int MinBBusStudents
        {
            get { return _minBBusStudents; }
            set { _minBBusStudents = value; }
        }
        /// <summary>
        /// Minimum number of engineering students who should be in B categeory
        /// </summary>
        [DisplayName("Min # of Engineering Students to be ranked as Desired (B):")]
        public int MinBEngStudents
        {
            get { return _minBEngStudents; }
            set { _minBEngStudents = value; }
        }
        /// <summary>
        /// Minimum number of students who should be in B category
        /// </summary>
        [DisplayName("Min total # of Students to be ranked as Desired (B):")]
        public int MinBStudents
        {
            get { return _minBStudents; }
            set { _minBStudents = value; }
        }

        /// <summary>
        /// Minimum number of projects that needed to be picked as th first choice by the students
        /// </summary>
        [DisplayName("A student should rank at least this many projects as first choice:")]
        public int MinFirstProjects
        {
            get { return _minFirstProjects; }
            set { _minFirstProjects = value; }
        }
        /// <summary>
        /// Maximum number of business students who can be rejected
        /// </summary>
        [DisplayName("Max # of Business Students a company can reject (Ignored if set to -1):")]
        public int MaxRejectedBusStudents
        {
            get { return _maxRejectedBusStudents; }
            set { _maxRejectedBusStudents = value; }
        }
        /// <summary>
        /// Maximum number of engineering students whou can be rejected
        /// </summary>
        [DisplayName("Max # of Engineering Students a company can reject (Ignored if set to -1):")]
        public int MaxRejectedEngStudents
        {
            get { return _maxRejectedEngStudents; }
            set { _maxRejectedEngStudents = value; }
        }
        /// <summary>
        /// Maximum number of students who can be rejected
        /// </summary>
        [DisplayName("Max total # of Students a company can reject (Ignored if set to -1):")]
        public int MaxRejectedStudents
        {
            get { return _maxRejectedStudents; }
            set { _maxRejectedStudents = value; }
        }
        /// <summary>
        /// Max number of projects that can be rejected by a student
        /// </summary>
        [DisplayName("Max # of Projects a student can reject (Ignored if set to -1):")]
        public int MaxRejectedProjects
        {
            get { return _maxRejectedProjects; }
            set { _maxRejectedProjects = value; }
        }
        /// <summary>
        /// Decides whether students should rank projects in a continuum. E.g.: If there are projects in 1st and 4th category then there should be projects in 2nd & 3rd category as well.
        /// </summary>
        [DisplayName("Should students rank projects in continuum e.g. If there are projects in the 1st and 4th category then there should be projects in 2nd & 3rd category as well:")]
        public bool EnforceContinuousProjectRanking
        {
            get { return _enforceContinuousProjectRanking; }
            set { _enforceContinuousProjectRanking = value; }
        }
        /// <summary>
        /// Decides whether projects should rank students in a continuum. E.g.: If there are students in A and D category then there should be students in B & C category as well.
        /// </summary>
        [DisplayName("Should companies rank students in continuum. e.g. If there are students in Ideal(A) category and in Acceptable(C) category then there should be students in Desired (B) category as well:")]
        public bool EnforceContinuousStudentRanking
        {
            get { return _enforceContinuousStudentRanking; }
            set { _enforceContinuousStudentRanking = value; }
        }
        /// <summary>
        /// If the number of projects are less than this number then students can not reject any projects
        /// </summary>
        [DisplayName("A student has to interview for at least this many projects to be able to reject any projects:")]
        public int RejectedProjectThreshold
        {
            get { return _rejectedProjectThreshold; }
            set { _rejectedProjectThreshold = value; }
        }
        /// <summary>
        /// If the number of students are less than this number then projects can not reject any students
        /// </summary>
        [DisplayName("A company has to interview at least this many students for a project to be able to reject any students:")]
        public int RejectedStudentThreshold
        {
            get { return _rejectedStudentThreshold; }
            set { _rejectedStudentThreshold = value; }
        }
    }
}