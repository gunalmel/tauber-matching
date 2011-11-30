using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauberMatching.Models
{
    public class StudentDetailsModel
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public IList<ProjectDto> NotInterviewed { get; set; }
        public IList<ProjectDto> Interviewed { get; set; }
        public StudentDetailsModel() { }
        public StudentDetailsModel(int id, string studentFullName, IList<ProjectDto> notInterviewed, IList<ProjectDto> interviewed)
        {
            this.StudentId = id;
            this.FullName = studentFullName;
            this.NotInterviewed = notInterviewed;
            this.Interviewed = interviewed;
        }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
