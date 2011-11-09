using System;
using System.Net.Mail;
using TauberMatching.Models;
using System.Linq;

namespace TauberMatching.Services
{
    /// <summary>
    /// Helper class to utilize async smtp utility class of .NET framework to send email messages through smtp. Relies on web.config for smtp configuration.
    /// </summary>
    public class EmailService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmailService));

        private static readonly string MAIL_SERVER;
        private static readonly int MAIL_PORT;// = 587;
        private static readonly string MAIL_ACCOUNT;// = "gunalmel@gmail.com";
        private static readonly string MAIL_PWD;// = "smooth1022";
        private static readonly string MAIL_FROM;
        private static readonly bool ENABLE_SSL;// = true;
        private static readonly bool IS_MAIL_HTML;// = true;
        private static readonly bool IS_TESTING;
        private static readonly string PICKUP_DIR;
        private MailMessage _mail = new MailMessage();
        private SmtpClient _mailServer;

        static EmailService()
        {
            MAIL_SERVER = System.Configuration.ConfigurationManager.AppSettings["MailServer"];
            MAIL_ACCOUNT = System.Configuration.ConfigurationManager.AppSettings["MailAccount"];
            MAIL_PWD = System.Configuration.ConfigurationManager.AppSettings["MailPassword"];
            MAIL_PORT = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MailServerPort"]);
            ENABLE_SSL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SSLEnabled"]);
            IS_MAIL_HTML = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMailBodyHtml"]);
            IS_TESTING = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsTesting"]);
            MAIL_FROM = new MatchingDB().ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterEmail)).Value;

            if(System.Web.HttpContext.Current==null)
                PICKUP_DIR = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            else
                PICKUP_DIR = System.Web.HttpContext.Current.Server.MapPath("~/");
            //PICKUP_DIR = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase); //Returns in URI form (starst with file:// which can not be used by smtpClient as pickup directory)
            PICKUP_DIR = System.IO.Path.Combine(PICKUP_DIR,"App_Data","emails");
            if (!System.IO.Directory.Exists(PICKUP_DIR))
                System.IO.Directory.CreateDirectory(PICKUP_DIR);
        }

        //TODO using Quartz implement a scheduled Email task using a db queue.
        public EmailService(string to, string subject, string body)
        {
            CreateMessage(to, subject, body);
            InitializeSmtpServer();
        }
        private void CreateMessage(string to, string subject, string body)
        {
            _mail.From = new MailAddress(MAIL_FROM);
            _mail.To.Add(to);
            _mail.Subject = subject;
            _mail.Body = body;
            _mail.IsBodyHtml = IS_MAIL_HTML;
        }
        private void InitializeSmtpServer()
        {
            if (IS_TESTING)
                TestLevelMailSetup();
            else
                ProductionLevelMailSetup(); 
        }

        private void TestLevelMailSetup()
        {
            _mailServer = new SmtpClient(MAIL_SERVER);
            _mailServer.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            _mailServer.PickupDirectoryLocation = PICKUP_DIR;
            //System.Configuration.ConfigurationManager.AppSettings["MailPickupDirectory"];
        }

        private void ProductionLevelMailSetup()
        {
            _mailServer = new SmtpClient(MAIL_SERVER);
            _mailServer.Credentials = new System.Net.NetworkCredential(MAIL_ACCOUNT, MAIL_PWD);
            _mailServer.Port = MAIL_PORT;
            _mailServer.EnableSsl = ENABLE_SSL;
        }
        public void SendMessage()
        {
            try
            {
                _mailServer.Send(_mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}