using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

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
        private int _maxRejectedBusStudents;
        private int _maxRejectedEngStudents;
        private int _maxRejectedStudents;
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
            String value = thisType.GetProperty(propName).GetValue(this, null).ToString();
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
        public int MaxRejectedBusStudents
        {
            get { return _maxRejectedBusStudents; }
            set { _maxRejectedBusStudents = value; }
        }
        /// <summary>
        /// Maximum number of engineering students whou can be rejected
        /// </summary>
        public int MaxRejectedEngStudents
        {
            get { return _maxRejectedEngStudents; }
            set { _maxRejectedEngStudents = value; }
        }
        /// <summary>
        /// mMax number of projects that can be rejected by a student
        /// </summary>
        public int MaxRejectedProjects
        {
            get { return _maxRejectedProjects; }
            set { _maxRejectedProjects = value; }
        }
        /// <summary>
        /// Maximum number of students whou can be rejected
        /// </summary>
        public int MaxRejectedStudents
        {
            get { return _maxRejectedStudents; }
            set { _maxRejectedStudents = value; }
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
        public int RejectedProjectThreshold
        {
            get { return _rejectedProjectThreshold; }
            set { _rejectedProjectThreshold = value; }
        }
        /// <summary>
        /// If the number of students are less than this number then projects can not reject any students
        /// </summary>
        public int RejectedStudentThreshold
        {
            get { return _rejectedStudentThreshold; }
            set { _rejectedStudentThreshold = value; }
        }
    }
}