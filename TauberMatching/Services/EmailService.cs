﻿using System;
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
        private static readonly int MAIL_PORT;
        private static readonly string MAIL_ACCOUNT;
        private static readonly string MAIL_PWD;
        private static readonly string MAIL_FROM;
        private static readonly string MAIL_FROM_INSTITUTE;
        private static readonly string MAIL_FROM_ADMIN;
        private static readonly bool ENABLE_SSL;
        private static readonly bool IS_MAIL_HTML;
        private static readonly bool IS_TESTING;
        private static readonly string PICKUP_DIR;
        private MailMessage _mail = new MailMessage();
        private SmtpClient _mailServer;

        static EmailService()
        {
            EmailConfiguration emailConfig = ConfigurationService.GetEmailConfigParameters();
            MAIL_SERVER = emailConfig.MailServer;//System.Configuration.ConfigurationManager.AppSettings["MailServer"];
            MAIL_ACCOUNT = emailConfig.MailAccount;//System.Configuration.ConfigurationManager.AppSettings["MailAccount"];
            MAIL_PWD = emailConfig.MailPassword;//System.Configuration.ConfigurationManager.AppSettings["MailPassword"];
            MAIL_PORT = emailConfig.MailServerPort;//Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MailServerPort"]);
            ENABLE_SSL = emailConfig.IsSSLEnabled;//Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SSLEnabled"]);
            IS_MAIL_HTML = emailConfig.IsMailBodyHtml;//Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMailBodyHtml"]);
            IS_TESTING = emailConfig.IsTesting;//Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsTesting"]);
            using (MatchingDB db = new MatchingDB())
            {
                MAIL_FROM = db.ConfigParameters.First(c => c.Id == ((int)ConfigEnum.SiteMasterEmail)).Value;                              
                MAIL_FROM_INSTITUTE = "Tauber Institute Matching Application";
                MAIL_FROM_ADMIN = db.ConfigParameters.First(c => c.Name == "SiteMasterFirstName").Value + " " + db.ConfigParameters.First(c => c.Name == "SiteMasterLastName").Value;
            }
            
            if(System.Web.HttpContext.Current==null)
                PICKUP_DIR = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            else
                PICKUP_DIR = System.Web.HttpContext.Current.Server.MapPath("~/");
            //PICKUP_DIR = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase); //Returns in URI form (starst with file:// which can not be used by smtpClient as pickup directory)
            PICKUP_DIR = System.IO.Path.Combine(PICKUP_DIR,"App_Data","emails");
            if (!System.IO.Directory.Exists(PICKUP_DIR))
                System.IO.Directory.CreateDirectory(PICKUP_DIR);
        }

        public EmailService(string to, string subject, string body)
        {
            CreateMessage(to, subject, body);
            InitializeSmtpServer();
        }
        private void CreateMessage(string to, string subject, string body)
        {
            _mail.From = new MailAddress(MAIL_FROM, MAIL_FROM_INSTITUTE);
            _mail.ReplyToList.Add(new MailAddress(MAIL_FROM, MAIL_FROM_ADMIN));
            try
            {
                if (to.Contains(','))
                    foreach (string receiver in to.Split(','))
                        _mail.To.Add(receiver);
                else
                    _mail.To.Add(to);
            }
            catch (Exception ex)
            {
                log.Error("The e-mail address/es in \"ToString\" field is/are invalid. Please check the last data entry box under \"Email Configuration\" tab to make sure that if there are multipe e-mail addresses are specified theya are sparated by \",\" not by \";\"",ex);
                throw ex;
            }
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
            _mailServer = new SmtpClient(MAIL_SERVER, MAIL_PORT);
            _mailServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            _mailServer.Credentials = new System.Net.NetworkCredential(MAIL_ACCOUNT, MAIL_PWD);
            _mailServer.EnableSsl = ENABLE_SSL;
        }
        public String SendMessage()
        {
            String status = EmailStatus.Failed.ToString();
            try
            {
                _mailServer.Send(_mail);
                status = EmailStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                Exception e = ex.InnerException == null ? ex : ex.InnerException;
                log.Error("Error while sending email using SmtpClient ", e);
            }
            return status;
        }
    }
    public enum EmailStatus { Success, Failed };
}