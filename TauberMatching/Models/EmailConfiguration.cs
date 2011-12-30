using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TauberMatching.Models
{
    public class EmailConfiguration
    {
        private string _mailServer;
        [DisplayName("Email Server:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the SMTP server to be used for sending notification e-mails !")]
        public string MailServer
        {
            get { return _mailServer; }
            set { _mailServer = value; }
        }     
        private int _mailServerPort;
        [DisplayName("Email Server Port:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the port to programmatically access the SMTP server to send notification emails !")]
        public int MailServerPort
        {
            get { return _mailServerPort; }
            set { _mailServerPort = value; }
        }
        private bool _isSSLEnabled;
        [DisplayName("Is SSL Enabled for the Email Server:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the email if SSL is enabled for SMTP server to send emails !")]
        public bool IsSSLEnabled
        {
            get { return _isSSLEnabled; }
            set { _isSSLEnabled = value; }
        }
        private bool _isMailBodyHtml;
        [DisplayName("Is Email Body HTML:")]
        [Required(ErrorMessage = "Mandatory Field: Specify if your notification e-mails will include HTML markup in their bodies !")]
        public bool IsMailBodyHtml
        {
            get { return _isMailBodyHtml; }
            set { _isMailBodyHtml = value; }
        }
        private string _mailAccount;
        [DisplayName("Email Server Account:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the user account to authenticate against SMTP server to be able to send emails !")]
        public string MailAccount
        {
            get { return _mailAccount; }
            set { _mailAccount = value; }
        }
        private string _mailPassword;
        [DisplayName("Email Server Account Password:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the user account password to authenticate against SMTP server to be able to send emails !")]
        public string MailPassword
        {
            get { return _mailPassword; }
            set { _mailPassword = value; }
        }
        private bool _isTesting;
        [DisplayName("Would you like to run application in test mode (no e-mails will be sent from the application):")]
        [Required(ErrorMessage = "Mandatory Field: You must specify if you are using the application in test mode in which case no notification e-mails will be sent !")]
        public bool IsTesting
        {
            get { return _isTesting; }
            set { _isTesting = value; }
        }
        private string _projectAccessUrlEmailSubject;
        [DisplayName("Subject of the Email that will notify project contacts of the unique access URL:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the subject phrase for the notification e-mail that will notify project contacts of the access url to use the application to rank the students !")]
        public string ProjectAccessUrlEmailSubject
        {
            get { return _projectAccessUrlEmailSubject; }
            set { _projectAccessUrlEmailSubject = value; }
        }
        private string _projectAccessUrlEmailBody;
        [DisplayName("Body of the Email that will notify project contacts of the unique access URL:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the e-mail body for the notification e-mail that will notify project contacts of the access url to use the application to rank the students !")]
        public string ProjectAccessUrlEmailBody
        {
            get { return _projectAccessUrlEmailBody; }
            set { _projectAccessUrlEmailBody = value; }
        }      
        private string _studentAccessUrlEmailSubject;
        [DisplayName("Subject of the Email that will notify students of the unique access URL:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the subject phrase for the notification e-mail that will notify students of the access url to use the application to rank the projects !")]
        public string StudentAccessUrlEmailSubject
        {
            get { return _studentAccessUrlEmailSubject; }
            set { _studentAccessUrlEmailSubject = value; }
        }
        private string _studentAccessUrlEmailBody;
        [DisplayName("Body of the Email that will notify students of the unique access URL:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the e-mail body for the notification e-mail that will notify students of the access url to use the application to rank the projects !")]
        public string StudentAccessUrlEmailBody
        {
            get { return _studentAccessUrlEmailBody; }
            set { _studentAccessUrlEmailBody = value; }
        }
        private string _accessUrlEmailHeader;
        [DisplayName("Access URL Email Body Header Section:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the fixed header section of the e-mail body for the notification e-mail that will notify project contacts and students of the access url to use the application to submit rankings !")]
        public string AccessUrlEmailHeader
        {
            get { return _accessUrlEmailHeader; }
            set { _accessUrlEmailHeader = value; }
        }
        private string _accessUrlEmailFooter;
        [DisplayName("Access URL Email Body Footer Section:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the fixed footer section of the e-mail body for the notification e-mail that will notify project contacts and students of the access url to use the application to submit rankings !")]
        public string AccessUrlEmailFooter
        {
            get { return _accessUrlEmailFooter; }
            set { _accessUrlEmailFooter = value; }
        }
        private string _invalidAccessUrlMessage;
        [DisplayName("Invalid Access URL Error Message:")]
        [Required(ErrorMessage = "Mandatory Field: You must specify the error message to be displayed to the users if they use an invalid access url to access to the application. !")]
        public string InvalidAccessUrlMessage
        {
            get { return _invalidAccessUrlMessage; }
            set { _invalidAccessUrlMessage = value; }
        }
        private string _mailPickupDirectory;

        public string MailPickupDirectory
        {
            get { return _mailPickupDirectory; }
            set { _mailPickupDirectory = value; }
        }
        private string _rankProjectsController;

        public string RankProjectsController
        {
            get { return _rankProjectsController; }
            set { _rankProjectsController = value; }
        }
        private string _rankStudentsController;

        public string RankStudentsController
        {
            get { return _rankStudentsController; }
            set { _rankStudentsController = value; }
        }
        private string _confirmationEmailReceivers;
        [DisplayName("Confirmation Email Receivers:")]
        [Required(ErrorMessage = "Mandatory Field: You must enter the list of e-mail accounts separated by comma character that will receive ranking submission confirmation e-mails !")]
        public string ConfirmationEmailReceivers
        {
            get { return _confirmationEmailReceivers; }
            set { _confirmationEmailReceivers = value; }
        }
        private Type thisType = typeof(EmailConfiguration);
        public EmailConfiguration() { }
        public EmailConfiguration(IQueryable<ConfigParameter> parameters)
        {
            foreach (var param in parameters)
            {
                if (param.Value != null && param.Value.Length > 0)
                {
                    PropertyInfo prop = thisType.GetProperty(param.Name);
                    Type t = prop.PropertyType;
                    Object o = Convert.ChangeType(param.Value, t);
                    prop.SetValue(this, o, null);
                }
            }
        }
        private ConfigParameter GetConfigParameter(EmailConfigEnum name)
        {
            int paramId = (int)name;
            String propName = Enum.GetName(typeof(EmailConfigEnum), name);
            var valueObject = thisType.GetProperty(propName).GetValue(this, null);
            String value = String.Empty;
            if (valueObject != null)
                value = valueObject.ToString();
            return new ConfigParameter(name, value);
        }
        /// <summary>
        /// Gets or sets instance properties by ConfigParameter object
        /// </summary>
        /// <param name="configParam">Configuration parameter as specified by the ConfigEnum</param>
        /// <returns>Instance of Configuration parameter object to manage persistence</returns>
        public ConfigParameter this[EmailConfigEnum configParam]
        {
            get { return GetConfigParameter(configParam); }
            set
            {
                if (value.Value != null && value.Value.Length > 0)
                {
                    PropertyInfo prop = thisType.GetProperty(Enum.GetName(typeof(ConfigEnum), configParam));
                    Type t = prop.PropertyType;
                    Object o = Convert.ChangeType(value.Value, t);
                    prop.SetValue(this, o, null);
                }
            }
        }
        /// <summary>
        /// Iterating through not null properties builds the collection of config parameters by mapping properties of this instance to named config parameters
        /// </summary>
        /// <returns>Enumeration of ConfigParameter objects that represents persistable application configuration settings.</returns>
        public IEnumerable<ConfigParameter> GetConfigParameters()
        {
            IList<ConfigParameter> configParameters = new List<ConfigParameter>();
            foreach (EmailConfigEnum e in Enum.GetValues(typeof(EmailConfigEnum)))
            {
                configParameters.Add(this[e]);
            }
            return configParameters;
        }
    }
}
