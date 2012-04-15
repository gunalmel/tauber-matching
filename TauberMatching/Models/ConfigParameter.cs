using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    /// <summary>
    /// Application parameter name/value pair that sets business rule parameters and application configuration parameters for Matching app.
    /// </summary>
    public class ConfigParameter
    {
        public ConfigParameter() { }
        /// <summary>
        /// Instantiates a named config parameter whose value is unset
        /// </summary>
        /// <param name="name">Name of the parameter as specified by ConfigEnum</param>
        public ConfigParameter(ConfigEnum name)
        {
            this.Id = (int)name;
            this.Name = Enum.GetName(typeof(ConfigEnum), name);
        }
        public ConfigParameter(EmailConfigEnum name)
        {
            this.Id = (int)name;
            this.Name = Enum.GetName(typeof(EmailConfigEnum), name);
        }
        public ConfigParameter(EmailConfigEnum name, String value)
            : this(name)
        {
            this.Value = value;
        }
        public ConfigParameter(ConfigEnum name, String value):this(name)
        {
            this.Value = value;
        }
        /// <summary>
        /// Id of config parameters are specified by COnfigEnum
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int Id{get;set;}
        /// <summary>
        /// Application configuration parameter name as specified by ConfigEnum. Property is read-only, if needed to be set then either constructor or SetName method should be used.
        /// </summary>
        [MaxLength(64, ErrorMessage = "Application configuration parameter name can be at most 64 characters long. See ConfigEnum in ConfigParameter file for possible values.")]
        public String Name { get; private set; }
        /// <summary>
        /// Setter for the read-only property name. Property is made read-only to constrain the parameter names that can be specified.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(ConfigEnum name) { this.Name = Enum.GetName(typeof(ConfigEnum), name);}

        private string _value;
        /// <summary>
        /// Value set for the application parameter specified by Name property
        /// </summary>
        [MaxLength(2048, ErrorMessage = "Application configuration parameter value can be at most 2048 characters long.")]
        public String Value { get { return _value; } set { this._value = value; } }

        /// <summary>
        /// Returns a string representation of configuration parameter value that can be used to produce a js variable assigment statement.
        /// </summary>
        [NotMapped]
        public String JsValue
        {
            get
            {
                Int32 intValue;
                string value;
                if (new[] { "true", "false"}.Contains(_value.ToLower()) || Int32.TryParse(_value, out intValue))
                    value = _value.ToLower();
                else
                    value = "\"" + _value + "\"";
                return value;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id.GetHashCode();
                result = (result * 397) ^ Value.GetHashCode();
                return result;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                throw new NullReferenceException("obj is null");
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ConfigParameter)) return false;
            var param = (ConfigParameter)obj;
            if (param.Id == this.Id && param.Value == this.Value)
                return true;
            return false;
        } 
    }
    
    public enum ConfigEnum
    {
        /// <summary>
        /// First name of the person whose contact information is going to be sent with notification emails
        /// </summary>
        SiteMasterFirstName=5,
        /// <summary>
        /// Last name of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        SiteMasterLastName=10,
        /// <summary>
        /// Email of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        SiteMasterEmail=15,
        /// <summary>
        /// Phone# of the person whose contact information is going to be sent with the notification emails
        /// </summary>
        SiteMasterPhone=20,
        /// <summary>
        /// Minimum number of business students who should be in A categeory
        /// </summary>
        MinABusStudents=25,
        /// <summary>
        /// Minimum number of engineering students who should be in A categeory
        /// </summary>
        MinAEngStudents=30,
        /// <summary>
        /// Minimum number of students who should be in A category
        /// </summary>
        MinAStudents=35,
        /// <summary>
        /// Min number of Busines students who should be ranked as B (desired)
        /// </summary>
        MinBBusStudents = 36,
        /// <summary>
        /// Min number of Engineering students who should be ranked as B (desired)
        /// </summary>
        MinBEngStudents = 37,
        /// <summary>
        /// Min number of students who should be ranked as B (desired)
        /// </summary>
        MinBStudents = 38,
        /// <summary>
        /// Maximum number of business students who can be rejected
        /// </summary>
        MaxRejectedBusStudents=40,
        /// <summary>
        /// Maximum number of engineering students whou can be rejected
        /// </summary>
        MaxRejectedEngStudents=45,
        /// <summary>
        /// Maximum number of students who can be rejected
        /// </summary>
        MaxRejectedStudents=50,
        /// <summary>
        /// If the number of students are less than this number then projects can not reject any students
        /// </summary>
        RejectedStudentThreshold=55,
        /// <summary>
        /// Minimum # of projects that needed to be pciked as the first preference
        /// </summary>
        MinFirstProjects=56,
        /// <summary>
        /// Maximun number of projects that can be rejected by a student
        /// </summary>
        MaxRejectedProjects=60,
        /// <summary>
        /// If the number of projects are less than this number then students can not reject any projects
        /// </summary>
        RejectedProjectThreshold=65,
        /// <summary>
        /// Decides whether students should rank projects in a continuum. If there are projects in 1st and 4th category then there should be projects in 2nd and 3rd category as well.
        /// </summary>
        EnforceContinuousProjectRanking=70,
        /// <summary>
        /// Decides whether projects should rank students in a continuum. If there are students in A and D category then there should be students in B and C category as well.
        /// </summary>
        EnforceContinuousStudentRanking=75
    }
    public enum EmailConfigEnum
    {
        MailServer=100,
        MailServerPort=105,
        IsSSLEnabled=110,
        IsMailBodyHtml=115,
        MailAccount=120,
        MailPassword=125,
        IsTesting=130,
        MailPickupDirectory=135,
        RankProjectsController=140,
        RankStudentsController=145,
        InvalidAccessUrlMessage=150,
        ProjectAccessUrlEmailSubject=155,
        ProjectAccessUrlEmailBody=160,
        StudentAccessUrlEmailSubject=165,
        StudentAccessUrlEmailBody=170,
        EmailHeader=175,
        EmailFooter=180,
        ConfirmationEmailReceivers=185
    }
}