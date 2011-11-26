using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;
using System.Text;

namespace TauberMatching.Services
{
    public class ScoreService
    {
        public static IList<ScoreDetail> GetScoreDetailsFor(ContactType entityType)
        {
            using (var db = new MatchingDB())
            {
                string eType = entityType.ToString();
                return db.ScoreDetails.Where(sc => sc.ScoreFor == eType).ToList();
            }
        }

        /// <summary>
        /// Builds the js statements that will set the javascrit arrays that will hold project|sudent scores
        /// </summary>
        /// <param name="scoreDetails">List of score detail objects that will be used to generate the javascript statement to set js array </param>
        /// <returns>JS statemets setting up js arrays that hold project|student scores</returns>
        public static string GetScoreJavascriptArrayFromScoreDetails(IList<ScoreDetail> scoreDetails)
        {
            StringBuilder jsVariables = new StringBuilder();
            string jsStringArrayElementTemplate = "\"{0}\",";
            jsVariables.Append("var scoreList = [");
            foreach (ScoreDetail sd in scoreDetails)
            {
                jsVariables.AppendFormat(jsStringArrayElementTemplate, sd.Score);
            }
            jsVariables.Remove(jsVariables.Length - 1, 1);
            jsVariables.Append("];").AppendLine();

            return jsVariables.ToString();
        }
    }
}