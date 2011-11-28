using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Models
{
    /// <summary>
    /// Model class that will be used by admin console's project related details view
    /// </summary>
    public class ProjectDetailsModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IList<StudentDto> NotInterviewed { get; set; }
        public IList<StudentDto> Interviewed { get; set; }
        public ProjectDetailsModel() { }
        public ProjectDetailsModel(int projectId, string projectName, IList<StudentDto> notInterviewed,IList<StudentDto> interviewed) 
        {
            this.ProjectId = projectId;
            this.ProjectName = projectName;
            this.NotInterviewed = notInterviewed;
            this.Interviewed = interviewed;
        }
    }

    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return (FirstName != null || LastName != null) ? (FirstName + " " + LastName) : "";
            }
        }
        public string Degree { get; set; }
        public StudentDto() { }
        public StudentDto(int id, string firstName,string lastName,string degree) 
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Degree = degree;
        }
    }
}