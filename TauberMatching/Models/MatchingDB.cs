using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TauberMatching.Models
{
    public class MatchingDB : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentFeedback> StudentFeedbacks { get; set; }
        public DbSet<Matching> Matchings { get; set; }
        public DbSet<UserError> UserErrors { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<UploadEntity> UploadEntities { get; set; }
        public DbSet<ProjectReject> ProjectRejects { get; set; }
        public DbSet<ConfigParameter> ConfigParameters { get; set; }
        public DbSet<EmailQueueMessage> EmailQueueMessages { get; set; }
        public DbSet<ScoreDetail> ScoreDetails { get; set; }
        public MatchingDB()
        {
            Database.SetInitializer<MatchingDB>(new DropCreateDatabaseIfModelChanges<MatchingDB>());
        }
        
    }
}