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

        public RankStudentsIndexModel(String projectName, IDictionary<ScoreDetail, IList<Student>> scoreGroupedStudents, ProjectScoreStudentCountMatrix projectScoreStudentCountMatrix)
        {
            ProjectName = projectName;
            IsError = false;
            ScoreGroupedStudents = scoreGroupedStudents;
            ProjectScoreStudentCountMatrix = projectScoreStudentCountMatrix;
        }
        public RankStudentsIndexModel(bool isError, String errorMessage)
        {
            IsError = isError;
            ErrorMessage = errorMessage;
        }
    }
}