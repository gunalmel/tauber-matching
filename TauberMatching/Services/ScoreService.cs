using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TauberMatching.Models;

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
    }
}