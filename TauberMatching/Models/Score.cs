using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    /// <summary>
    /// Class that will be used by UI to build score buckets for Students and Projects
    /// </summary>
    public class ScoreDetail
    {
        private string _scoreFor;       
        private string _score;
        private string _scoreTypeDisplay;

        public int Id { get; set; }
        /// <summary>
        /// Implies who's using the scores to rank the other entity. Possible values: Project|Student
        /// </summary>
        [MaxLength(32, ErrorMessage = "ScoreFor can be at most 32 characters long. See ContactType enum for possible values")]
        public string ScoreFor
        {
            get { return _scoreFor; }
            set { _scoreFor = value; }
        } 
        /// <summary>
        /// The score string that will be stored in the database as it will also be reported in the Excel output matrix..
        /// </summary>
        [MaxLength(32, ErrorMessage = "Score can be at most 32 characters long.")]
        public string Score
        {
            get { return _score; }
            set { _score = value; }
        }
        /// <summary>
        /// The score title that will be used when displaying the score buckets on the UI.
        /// </summary>
        [MaxLength(256, ErrorMessage = "Score type display text can be at most 256 characters long.")]
        public string ScoreTypeDisplay
        {
            get { return _scoreTypeDisplay; }
            set { _scoreTypeDisplay = value; }
        }

    }
}