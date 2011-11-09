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
        public String Name { get; private set; }
        /// <summary>
        /// Setter for the read-only property name. Property is made read-only to constrain the parameter names that can be specified.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(ConfigEnum name) { this.Name = Enum.GetName(typeof(ConfigEnum), name);}
        /// <summary>
        /// Value set for the application parameter specified by Name property
        /// </summary>
        public String Value { get; set; }

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
        /// Maximum number of business students who can be rejected
        /// </summary>
        MaxRejectedBusStudents=40,
        /// <summary>
        /// Maximum number of engineering students whou can be rejected
        /// </summary>
        MaxRejectedEngStudents=45,
        /// <summary>
        /// Maximum number of students whou can be rejected
        /// </summary>
        MaxRejectedStudents=50,
        /// <summary>
        /// If the number of students are less than this number then projects can not reject any students
        /// </summary>
        RejectedStudentThreshold=55,
        /// <summary>
        /// Maximun number of projects that can be rejected by a student
        /// </summary>
        MaxRejectedProjects=60,
        /// <summary>
        /// If the number of projects are less than this number then students can not reject any projects
        /// </summary>
        RejectedProjectThreshold=65,
        /// <summary>
        /// Decides whether students should rank projects in a continuum. E.g.: If there are projects in 1st and 4th category then there should be projects in 2nd & 3rd category as well.
        /// </summary>
        EnforceContinuousProjectRanking=70,
        /// <summary>
        /// Decides whether projects should rank students in a continuum. E.g.: If there are students in A and D category then there should be students in B & C category as well.
        /// </summary>
        EnforceContinuousStudentRanking=75,
    }
}