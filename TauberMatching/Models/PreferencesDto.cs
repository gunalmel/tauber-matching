using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauberMatching.Models
{
    /// <summary>
    /// Data transfer object used to transfer projects ranking students data from UI to Web Service to persist Projects ranking students results.
    /// </summary>
    [Serializable]
    public class ProjectPreferencesDto
    {
        public int ProjectId { get; set; }
        public string ProjectGuid { get; set; }
        public IList<ProjectScoreDto> ProjectPreferences { get; set; }
        public IList<ProjectRejectDto> ProjectRejects { get; set; }
        public string Feedback{get;set;}
    }

    [Serializable]
    public class ProjectScoreDto
    {
        public int StudentId { get; set; }
        public string Score { get; set; }
    }
    [Serializable]
    public class ProjectRejectDto
    {
        public int StudentId { get; set; }
        public String Reason { get; set; }
    }

    /// <summary>
    /// Data transfer object used to transfer students ranking projects data from UI to Web Service to persist Projects ranking students results.
    /// </summary>
    [Serializable]
    public class StudentPreferencesDto
    {
        public int StudentId { get; set; }
        public string StudentGuid { get; set; }
        public IList<StudentScoreDto> StudentPreferences { get; set; }
        public IList<StudentFeedbackDto> StudentFeedback { get; set; }
        public string OtherComments { get; set; }
    }

    [Serializable]
    public class StudentScoreDto
    {
        public int ProjectId { get; set; }
        public string Score { get; set; }
    }
    [Serializable]
    public class StudentFeedbackDto
    {
        public int ProjectId { get; set; }
        public String Type { get; set; } // P: Psoitive, C: Constructive
        public String FeedbackScore { get; set; }
    }
}
