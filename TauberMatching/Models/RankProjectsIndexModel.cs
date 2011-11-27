using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauberMatching.Models
{
    public class RankProjectsIndexModel
    {
        private string _studentName;

        public string StudentName
        {
            get { return _studentName; }
            set { _studentName = value; }
        }
        private IDictionary<ScoreDetail, IList<Project>> scoreGroupedProjects;

        public IDictionary<ScoreDetail, IList<Project>> ScoreGroupedProjects
        {
            get { return scoreGroupedProjects; }
            set { scoreGroupedProjects = value; }
        }
        private bool _isError;

        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; }
        }
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public int StudentId { get; set; }

        public string Guid { get; set; }

        public string OtherComments { get; set; }

        public IList<Project> ProjectsNotInterviewed { get; set; }

        public RankProjectsIndexModel(int studentId, string guid, string studentName, IDictionary<ScoreDetail, IList<Project>> scoreGroupedProjects, string otherComments, IList<Project> projectsNotInterviewed)
        {
            this.StudentId = studentId;
            this.Guid = guid;
            this._studentName = studentName;
            this.IsError = false;
            this.scoreGroupedProjects = scoreGroupedProjects;
            this.OtherComments = otherComments;
            this.ProjectsNotInterviewed = projectsNotInterviewed;
        }

        public RankProjectsIndexModel(bool isError, string errorMessage)
        {
            this.IsError = isError;
            this.ErrorMessage = errorMessage;
        }
    }
}
