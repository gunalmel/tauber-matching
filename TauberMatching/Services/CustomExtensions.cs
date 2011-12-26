using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;

namespace TauberMatching.Services
{
    /// <summary>
    /// Class encapsulating all extension methods.
    /// </summary>
    public static class CustomExtensions
    {
            /// <summary>
            /// Converts first letters of every word in a string to upper case.
            /// </summary>
            /// <param name="strText">The string instance on which this extension method will be called</param>
            /// <returns>A string in which every word's first letter is converted to upper case</returns>
            public static string InitCap(this String strText)
            {
                return new CultureInfo("en").TextInfo.ToTitleCase(strText.ToLower());
            }
            /// <summary>
            /// Used to modify properties of an object returned from a LINQ query
            /// </summary>
            public static TSource Set<TSource>(this TSource input, Action<TSource> updater)
            {
                updater(input);
                return input;
            }
    }
}