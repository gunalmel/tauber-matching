using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Models
{
    /// <summary>
    /// The model object that will be sent to the Index view returned by RankStudentsController
    /// </summary>
    public class RankStudentsIndexModel
    {
        private int _projectId;

        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        private string _guid;

        public string Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        private bool _isError;

        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; }
        }
        private IDictionary<ScoreDetail, IList<Student>> _scoreGroupedStudents;

        public IDictionary<ScoreDetail, IList<Student>> ScoreGroupedStudents
        {
            get { return _scoreGroupedStudents; }
            set { _scoreGroupedStudents = value; }
        }
        private ProjectScoreStudentCountMatrix _projectScoreStudentCountMatrix;

        public ProjectScoreStudentCountMatrix ProjectScoreStudentCountMatrix
        {
            get { return _projectScoreStudentCountMatrix; }
            set { _projectScoreStudentCountMatrix = value; }
        }

        private IDictionary<Student,string> _rejects;

        /// <summary>
        /// Dictionary key is the student degree, value is the reason
        /// </summary>
        public IDictionary<Student, string> Rejects
        {
            get { return _rejects; }
            set { _rejects = value; }
        }

        private string _feedback;

        public string Feedback
        {
            get { return _feedback; }
            set { _feedback = value; }
        }

        private IDictionary<StudentDegree, int> _studentCountDict;

        /// <summary>
        /// Key is the degree, value is the student count in that degree
        /// </summary>
        public IDictionary<StudentDegree, int> StudentCountDict
        {
            get { return _studentCountDict; }
            set { _studentCountDict = value; }
        }

        private string _uiJsStatements;

        /// <summary>
        /// Js variable assigment statements that will be injected into the veiw to be consumed by Js files
        /// </summary>
        public string UIJsStatements
        {
            get { return _uiJsStatements; }
            set { _uiJsStatements = value; }
        }

        public RankStudentsIndexModel(int projectId,String guid,String projectName, IDictionary<ScoreDetail, IList<Student>> scoreGroupedStudents, ProjectScoreStudentCountMatrix projectScoreStudentCountMatrix, IDictionary<Student, string> projectRejects, string feedback, IDictionary<StudentDegree, int> studentCountDict, string uiRules)
        {
            ProjectId = projectId;
            Guid = guid;
            ProjectName = projectName;
            IsError = false;
            ScoreGroupedStudents = scoreGroupedStudents;
            ProjectScoreStudentCountMatrix = projectScoreStudentCountMatrix;
            Rejects=projectRejects;
            Feedback = feedback;
            StudentCountDict = studentCountDict;
            UIJsStatements = uiRules;
        }
        public RankStudentsIndexModel(bool isError, String errorMessage)
        {
            IsError = isError;
            ErrorMessage = errorMessage;
        }
    }
}