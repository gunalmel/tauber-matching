using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using TauberMatching.Services;

namespace TauberMatching.Models
{
    /// <summary>
    /// All automatically generated e-mails are built using this class.
    /// Whenever an email is sent by the user or the system the email message is dropped in the database to form a queue that will be fed into Quartz scheduled jobs
    /// </summary>
    public class EmailQueueMessage
    {
        #region Constants
        /// <summary>
        /// The subject line of the notification e-mail that is sent to the project contacts to inform them how to access the application.
        /// </summary>
        private static readonly string PROJECT_URL_EMAIL_SUBJECT = System.Configuration.ConfigurationManager.AppSettings["ProjectAccessUrlSubject"];
        /// <summary>
        /// The subject line of the notification e-mail that is sent to the students to inform them how to access the application.
        /// </summary>
        private static readonly string STUDENT_URL_EMAIL_SUBJECT = System.Configuration.ConfigurationManager.AppSettings["StudentAccessUrlSubject"];
        /// <summary>
        /// Notification e-mail body template that will be sent to the project contacts to inform them of Tauber Matching Web Application access url
        /// </summary>
        private static readonly string PROJECT_URL_EMAIL_BODY = System.Configuration.ConfigurationManager.AppSettings["ProjectAccessUrlBody"];
        /// <summary>
        /// Notification e-mail body template that will be sent to the students to inform them of Tauber Matching Web Application access url
        /// </summary>
        private static readonly string STUDENT_URL_EMAIL_BODY = System.Configuration.ConfigurationManager.AppSettings["StudentAccessUrlBody"];
        /// <summary>
        /// Dicitonary that holds ConfigParameters that are related to site admin information (name, email, phone)
        /// </summary>
        private static readonly Dictionary<ConfigEnum, String> _siteAdminInfoDict;
        /// <summary>
        /// The variables in the header in order are: ContactFullName.
        /// </summary>
        private static readonly string _emailHeaderTemplate = System.Configuration.ConfigurationManager.AppSettings["NotificationEmailHeader"];
        /// <summary>
        /// The variables in the footer in order are:  SiteAdminFullName, SiteAdminEmail, UserFullName,SiteAdminPhone
        /// </summary>
        private static readonly string _emailFooterTemplate = System.Configuration.ConfigurationManager.AppSettings["NotificationEmailFooter"]; 
        #endregion
        #region Properties
        public int Id { get; set; }
        public int ContactId { get; set; }
        [MaxLength(32, ErrorMessage = "Contact type can be at most 32 characters long. See ContactType enum for possible values")]
        public string ContactType { get; set; }
        [MaxLength(128, ErrorMessage = "Contact first name can be at most 128 characters long.")]
        public string FirstName { get; set; }
        [MaxLength(128, ErrorMessage = "Contact last name can be at most 128 characters long.")]
        public string LastName { get; set; }
        public Guid Guid { get; set; }
        public string To { get; set; }
        [MaxLength(512, ErrorMessage = "Email subject can be at most 512 characters long.")]
        public string Subject { get; set; }
        public string Body { get; set; } 
        #endregion
        static EmailQueueMessage()
        {
            using (var db = new MatchingDB())
            {
                _siteAdminInfoDict = db.ConfigParameters
                                        .Where(c => new string[] { "SiteMasterFirstName", "SiteMasterLastName", "SiteMasterEmail", "SiteMasterPhone" }
                                        .Contains(c.Name))
                                        .ToDictionary(c => (ConfigEnum)Enum.Parse(typeof(ConfigEnum),c.Name), c => c.Value);
            }
        }
        #region Constructors
        public EmailQueueMessage() { }
        /// <summary>
        /// Helper constructor to be used by other constructors to set internal values
        /// </summary>
        private EmailQueueMessage(int contactId, string contactType, string firstName, string lastName, Guid guid, string to)
        {
            ContactId = contactId;
            ContactType = contactType;
            FirstName = firstName;
            LastName = lastName;
            Guid = guid;
            To = to;
        }
        /// <summary>
        /// Initializes user specified e-mail message.
        /// </summary>
        /// <param name="contactId">Database assigned unique identifier for whoever is being e-mailed (student or project contact)</param>
        /// <param name="contactType">String implying whether contact is student or project contact ("Student"|"Project")</param>
        /// <param name="firstName">First name of the contact being emailed</param>
        /// <param name="lastName">Last name of the contact being emailed</param>
        /// <param name="guid">The global unique identifier assigned to the contact to generate contact specific unique access url. Guid is generated at the the time when the databse record for the contact was created the first time. It stays the same throughout the updates to the contact</param>
        /// <param name="to">The e-mail address for the contact</param>
        /// <param name="subject">Subject line of the e-mail message</param>
        /// <param name="body">Message body of the email message</param>
        public EmailQueueMessage(int contactId, string contactType, string firstName, string lastName, Guid guid, string to, string subject, string body)
            : this(contactId, contactType, firstName, lastName, guid, to)
        {
            Subject = subject;
            Body = body;
        }
        /// <summary>
        /// Constructor to set subject and body sections of e-mail to the system generated values using pre-set e-mail templates. Used when system-generated notification e-mails are to be used to notify contacts. 
        /// Usage: EmailQueueService.EmailQueueMessage(new Contact(&lt;Project|Student&gt;),EmailType.&lt;Project|Student&gt;);
        /// </summary>
        /// <param name="type">Type of email template that will be use to generate system generated e-mail subject and body</param>
        public EmailQueueMessage(Contact contact, EmailType type)
            : this(contact.Id, contact.ContactType.ToString(), contact.FirstName, contact.LastName, contact.Guid, contact.Email)
        {
            Subject = GetEmailSubject(type);
            Body = GetEmailBody(contact.FirstName, contact.LastName, contact.Guid, type);
        } 
        #endregion
        private string GetEmailSubject(EmailType type)
        {
            string subject=null;
            switch (type)
            {
                case EmailType.ProjectAccess:
                    subject = PROJECT_URL_EMAIL_SUBJECT;
                    break;
                case EmailType.StudentAccess:
                    subject = STUDENT_URL_EMAIL_SUBJECT;
                    break;
                case EmailType.ProjectSubmit:
                    break;
                case EmailType.StudentSubmit:
                    break;
                default:
                    break;
            }
            return subject;
        }
        private string GetEmailBody(string firstName, string lastName,Guid guid,EmailType type)
        {
            #region Build Email Header & Footer
            string siteAdminFullName = _siteAdminInfoDict[ConfigEnum.SiteMasterFirstName] + " " + _siteAdminInfoDict[ConfigEnum.SiteMasterLastName];
            string contactFullName = firstName + " " + lastName;
            string emailHeader = String.Format(_emailHeaderTemplate, contactFullName);
            string emailFooter = String.Format(_emailFooterTemplate, siteAdminFullName, _siteAdminInfoDict[ConfigEnum.SiteMasterEmail], contactFullName, _siteAdminInfoDict[ConfigEnum.SiteMasterPhone]); 
            #endregion
            #region Build Email Body
            string body = String.Empty;
            switch (type)
            {
                case EmailType.ProjectAccess:
                    body = String.Format(PROJECT_URL_EMAIL_BODY, UrlHelper.GetAccessUrlForTheUser(guid, UrlType.Project));
                    break;
                case EmailType.StudentAccess:
                    body = String.Format(STUDENT_URL_EMAIL_BODY, UrlHelper.GetAccessUrlForTheUser(guid, UrlType.Student));
                    break;
                case EmailType.ProjectSubmit:
                    break;
                case EmailType.StudentSubmit:
                    break;
                default:
                    break;
            } 
            #endregion
            string message = emailHeader + body + emailFooter;
            return message;
        }      
    }
    /// <summary>
    /// Enum to instruct how to build the automatically generated e-mail messages.
    /// </summary>
    public enum EmailType { ProjectAccess, StudentAccess, ProjectSubmit, StudentSubmit}
}