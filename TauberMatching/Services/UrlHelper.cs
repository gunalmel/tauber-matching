using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Services
{
    public class UrlHelper
    {
        /// <summary>
        /// Controller name for student users to assign scores to the projects, that will be used to build the access Url that will be sent to the students
        /// </summary>
        private static readonly string RANK_PROJECT_CONTROLLER = System.Configuration.ConfigurationManager.AppSettings["RankProjectsController"];
        /// <summary>
        /// Controller name for project contacts to assign scores to the students, that will be used to build the access Url that will be sent to the project contacts
        /// </summary>
        private static readonly string RANK_STUDENT_CONTROLLER = System.Configuration.ConfigurationManager.AppSettings["RankStudentsController"];

        public static string GetApplicationRoot()
        {
            string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";
            string applicationRoot = HttpContext.Current.Request.ApplicationPath.Length == 1 ? "" : HttpContext.Current.Request.ApplicationPath;
            string url = protocol + server + port + applicationRoot;
            return url;
        }

        /// <summary>
        /// Returns the unique secret access url for the user.
        /// </summary>
        /// <param name="guid">Guid id assigned to the contact acting as unique, user sppecific access Url identifier</param>
        /// <param name="type">Type of user.</param>
        /// <returns></returns>
        public static string GetAccessUrlForTheUser(Guid guid, UrlType type)
        {
            var controller = (type == UrlType.Project ? RANK_STUDENT_CONTROLLER : RANK_PROJECT_CONTROLLER);
            string url = GetApplicationRoot()+controller+"/"+guid;
            return url;
        }
        /// <summary>
        /// Returns the unique secret access url for the user using a user provided appliation root url
        /// </summary>
        /// <param name="appRoot">Application root url that will be appended by relative access url</param>
        /// <param name="guid">Guid id assigned to the contact acting as unique, user sppecific access Url identifier</param>
        /// <param name="type">Type of user.</param>
        /// <returns></returns>
        public static string GetAccessUrlForTheUser(string appRoot, Guid guid, UrlType type)
        {
            var controller = (type == UrlType.Project ? RANK_PROJECT_CONTROLLER : RANK_STUDENT_CONTROLLER);
            string url = appRoot + controller + "/" + guid;
            return url;
        }

        public static void ExpirePageCache()
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            HttpContext.Current.Response.Cache.SetLastModified(DateTime.Now);
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
    }
    public enum UrlType
    {
        Project, Student
    }
}