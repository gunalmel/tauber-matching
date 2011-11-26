using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using TauberMatching.Models;

namespace TauberMatching.Services
{
    public class StudentDegreeService
    {
        /// <summary>
        /// Extracts a string that will be injcted in the page header to form a js statement to set an array holding list of student degrees.
        /// </summary>
        /// <returns></returns>
        public static string GetStudentDegreeJavaScritArray()
        {
            StringBuilder jsVariables = new StringBuilder();
            string jsStringArrayElementTemplate = "\"{0}\",";
            jsVariables.Append("var degreeList = [");
            foreach (StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
            {
                jsVariables.AppendFormat(jsStringArrayElementTemplate, degree.ToString());
            }
            jsVariables.Remove(jsVariables.Length - 1, 1);
            jsVariables.Append("];").AppendLine();
            return jsVariables.ToString();
        }
    }
}