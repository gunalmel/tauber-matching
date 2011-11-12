using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Models
{
    public class Matching
    {
        public Matching() { }
        public Matching(MatchingDTO mdto)
        {
            this.Project = new Project() { Id = mdto.ProjectId };
            this.Student = new Student() { Id = mdto.StudentId };
        }
        public int Id { get; set; }
        public Project Project { get; set; }
        public Student Student { get; set; }
        public string ProjectScore { get; set; }
        public string StudentScore { get; set; }
    }
    public class MatchingDTO
    {
        public int StudentId { get; set; }
        public int ProjectId { get; set; }
    }
}