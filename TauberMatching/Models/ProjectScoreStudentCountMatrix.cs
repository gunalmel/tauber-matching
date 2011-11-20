using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Services;

namespace TauberMatching.Models
{
    /// <summary>
    /// Data structure to store count of students grouped by degree who's been assigned a particular ranking score.
    /// </summary>
    public class ProjectScoreStudentCountMatrix
    {
        private IDictionary<String, IDictionary<StudentDegree, int>> _scoreStudentDegreeCountDictionary;

        public IDictionary<String, IDictionary<StudentDegree, int>> ScoreStudentDegreeCountDictionary
        {
            get 
            {
                if (_scoreStudentDegreeCountDictionary == null)
                {
                    _scoreStudentDegreeCountDictionary = new Dictionary<String, IDictionary<StudentDegree, int>>();
                    foreach (var sd in ProjectService.ProjectScoreDetails)
                    {
                        _scoreStudentDegreeCountDictionary.Add(sd.Score, new Dictionary<StudentDegree, int>());
                        foreach (StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
                            _scoreStudentDegreeCountDictionary[sd.Score].Add(degree, 0);
                    }
                }
                return _scoreStudentDegreeCountDictionary; 
            }
            set { _scoreStudentDegreeCountDictionary = value; }
        }

        public int this[string score, StudentDegree degree]
        {
            get
            {
                return ScoreStudentDegreeCountDictionary[score][degree];
            }
            set
            {
                ScoreStudentDegreeCountDictionary[score][degree] = value;
            }
        }
    }
}