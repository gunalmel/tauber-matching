using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauberMatching.Models
{
    /// <summary>
    /// Data transfer object used to transfer data from UI to Web Service to persist Projects ranking students results.
    /// </summary>
    [Serializable]
    public class PreferencesDto
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
}
