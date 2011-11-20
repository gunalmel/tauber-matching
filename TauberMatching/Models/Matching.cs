using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(32, ErrorMessage = "Score can be at most 32 characters long.")]
        public string ProjectScore { get; set; }
        [MaxLength(32, ErrorMessage = "Score can be at most 32 characters long.")]
        public string StudentScore { get; set; }
    }
    public class MatchingDTO
    {
        public int StudentId { get; set; }
        public int ProjectId { get; set; }
    }

    public enum ProjectScore { NoScore=0, A = 10, B = 20, C = 30, Reject = 40 }
    public enum StudentScore { NoScore=50, First = 60, Second = 70, Third = 80, Fourth = 90, Fifth=100, Reject=110 }
}